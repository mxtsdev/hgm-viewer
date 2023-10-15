using HgmViewer.Classes;
using HgmViewer.Formats;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static HgmViewer.Formats.Sltm;

namespace HgmViewer.Service
{
    /// <summary>
    /// Pack-files (.hpk) Service
    /// </summary>
    public class PackService
    {
        private readonly ConfigService _configService;
        private readonly ExportService _exportService;

        private List<TreeEntry> _entriesFlat;
        private List<TreeEntry> _entries;
        private Dictionary<string, TreeEntry[]> _entriesNameMap;
        private Dictionary<string, TreeEntry[]> _entriesFilePathMap;

        public IReadOnlyCollection<TreeEntry> EntriesFlat => _entriesFlat.AsReadOnly();
        public IReadOnlyCollection<TreeEntry> Entries => _entries.AsReadOnly();

        public PackService(
            ConfigService configService,
            ExportService exportService)
        {
            _configService = configService;
            _exportService = exportService;
        }

        public TreeEntry FindByName(string name)
        {
            _entriesNameMap ??= _entriesFlat
                    .Where(x => x.IsFile)
                    .OrderBy(x => x.PackFilePath.EndsWith("Fallbacks.hpk", StringComparison.OrdinalIgnoreCase))
                    .GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(x => x.Key, x => x.ToArray(), StringComparer.OrdinalIgnoreCase);

            return _entriesNameMap.TryGetValue(name, out var results) ? results?.Length >= 1 ? results[0] : null : null;
        }

        public TreeEntry[] FindByNames(IEnumerable<string> names)
        {
            return names.Select(x => FindByName(x)).ToArray();
        }

        public TreeEntry FindByFilePath(string filePath)
        {
            _entriesFilePathMap ??= _entriesFlat
                    .Where(x => x.IsFile)
                    .OrderBy(x => x.PackFilePath.EndsWith("Fallbacks.hpk", StringComparison.OrdinalIgnoreCase))
                    .GroupBy(x => x.FilePath, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(x => x.Key, x => x.ToArray(), StringComparer.OrdinalIgnoreCase);

            return _entriesFilePathMap.TryGetValue(filePath.Replace("/", @"\"), out var results) ? results?.Length >= 1 ? results[0] : null : null;
        }

        public TreeEntry[] FindByFilePaths(string[] filePaths)
        {
            return filePaths.Select(x => FindByFilePath(x)).ToArray();
        }

        public HgmDefinition UnpackHgm(string filePath, bool showErrorDialog = true)
        {
            if (filePath == null || !File.Exists(filePath) || Path.GetExtension(filePath)?.Equals(".hgm", StringComparison.OrdinalIgnoreCase) == false)
                return null;

            var def = new HgmDefinition
            {
                HgmFileName = Path.GetFileName(filePath),
                HgmFilePath = filePath,
            };

            def.HgmFileDirPath = Path.GetDirectoryName(def.HgmFilePath);
            def.HgmFileName_WithoutExt = GetFileNameWithoutExtensionHgm(def.HgmFilePath);

            // parse hgm
            return !Hgm_ParseHgm(ref def, showErrorDialog) ? null : def;
        }

        public HgmDefinition UnpackHgm(TreeEntry item, bool unpackMaterialAndTextures = false, bool showErrorDialog = true)
        {
            if (!item.IsFile || Path.GetExtension(item.FilePath)?.Equals(".hgm", StringComparison.OrdinalIgnoreCase) == false)
                return null;

            var unpackedFilePaths = UnpackFiles(item.PackFilePath, new[] { item.FilePath });
            if (!(unpackedFilePaths?.Count >= 1))
                return null;

            var def = new HgmDefinition
            {
                HgmFileName = item.Name,
                HgmFilePath = unpackedFilePaths[0],
            };

            def.HgmFileDirPath = Path.GetDirectoryName(def.HgmFilePath);
            def.HgmFileName_WithoutExt = GetFileNameWithoutExtensionHgm(def.HgmFilePath);

            // parse hgm
            if (!Hgm_ParseHgm(ref def, showErrorDialog)) return null;

            // process material
            if (unpackMaterialAndTextures)
            {
                // mtlbin from packs
                var mtlbinFileName = Hgm_GetMtlbinForHgm(def.HgmFileName);
                def.MtlbinFilePath = Hgm_GetMtlbin(mtlbinFileName);

                // parse mtlbin (if it exists)
                var mtl = Hgm_ProcessMaterial(def.MtlbinFilePath);
                if (mtl != null)
                {
                    def.Materials[0] = mtl;

                    // process all sub-material mtlbin(s) (if they exist)
                    if (unpackMaterialAndTextures)
                    {
                        string[] submtls = null;
                        if (mtl.Mtlbin.Mtl is Version0Struct mtl0)
                            submtls = mtl0.Submtl?.Where(x => x.Data.Len != 0).Select(x => x.Data.Str).ToArray();
                        else if (mtl.Mtlbin?.Mtl is Version1Struct mtl1)
                            submtls = mtl1.Submtl?.Where(x => x.RecType == 2).Select(x => x.Data.Str).ToArray();

                        if (submtls != null)
                        {
                            // unpack all mtlbins
                            var submtlEntries = Hgm_GetMtlbins(submtls.Select(x => Hgm_GetMtlbinForSubMtl(x)));
                            if (submtlEntries != null)
                            {
                                var submtlIdx = 0;
                                foreach (var entry in submtlEntries)
                                {
                                    submtlIdx++;

                                    if (entry.UnpackedFile == null) continue;

                                    // parse mtlbin (if it exists)
                                    var submtl = Hgm_ProcessMaterial(entry.UnpackedFile.FilePath);
                                    if (submtl != null)
                                        def.Materials[submtlIdx] = submtl;
                                }
                            }
                        }
                    }
                }

                // textures from packs
                Hgm_GetTextures(def.Materials.Values);

                // convert textures from dds
                Hgm_ConvertTextures(def.Materials.Values);
            }

            return def;
        }

        private HgmMaterialDefinition Hgm_ProcessMaterial(string mtlbinFilePath)
        {
            var (success, mtlbin) = Hgm_ParseMtlbin(mtlbinFilePath);
            if (success)
            {
                var mtl = new HgmMaterialDefinition
                {
                    MtlbinFilePath = mtlbinFilePath,
                    Mtlbin = mtlbin
                };

                return mtl;
            }

            return null;
        }

        public ExportedModelDefinition[] UnpackAndExportMultiple(TreeEntry[] hgms, IProgress<ExportAndUnpackProgress> progress, bool unpackMaterialAndTextures = false)
        {
            var results = new List<ExportedModelDefinition>();

            var n = 0;
            foreach (var item in hgms)
            {
                n++;

                var report = new ExportAndUnpackProgress
                {
                    Current = n,
                    Total = hgms.Length,
                    Action = "Exporting",
                    FileName = item.Name
                };

                if (!item.IsFile || Path.GetExtension(item.FilePath)?.Equals(".hgm", StringComparison.OrdinalIgnoreCase) == false)
                    continue;

                progress.Report(report);
                HgmDefinition unpacked;
                try
                { 
                    unpacked = UnpackHgm(item, unpackMaterialAndTextures);
                    if (unpacked == null) continue;
                }
                catch (Exception ex)
                {
                    progress.Report(report with { ErrorMessage = $"Failed to unpack Hgm: {ex}" });
                    continue;
                }

                try
                {
                    var exported = _exportService.ExportGlb(unpacked);
                    results.Add(exported);
                }
                catch (Exception ex)
                {
                    progress.Report(report with { ErrorMessage = $"Failed to export: {ex}" });
                }
            }

            progress.Report(new ExportAndUnpackProgress
            {
                Current = hgms.Length,
                Total = hgms.Length,
                Action = "Completed"
            });

            return results.ToArray();
        }

        private string Hgm_GetMtlbinForHgm(string hgmFileName)
        {
            // convert file name
            // form: MilitiaCostumeMale_Shirt_01_mesh.hgm
            // to:   Materials#MilitiaCostumeMale_Shirt_01_mesh.mtlbin

            return "Materials#" + GetFileNameWithoutExtensionHgm(hgmFileName) + ".mtlbin";
        }

        private string Hgm_GetMtlbinForSubMtl(string fileName)
        {
            // convert file name
            // from: MilitiaCostumeMale_Shirt_01_mesh.mtl.sub_0.mtl
            // to:   Materials#MilitiaCostumeMale_Shirt_01_mesh.mtl.sub_0.mtlbin

            return "Materials#" + GetFileNameWithoutExtensionHgm(fileName) + ".mtlbin";
        }

        private string Hgm_GetMtlbin(string mtlbinFileName)
        {
            var mtlbin = FindByName(mtlbinFileName);
            if (mtlbin == null) return null;

            var unpackedMtlbinFilePaths = UnpackFiles(mtlbin.PackFilePath, new[] { mtlbin.FilePath });
            return unpackedMtlbinFilePaths.FirstOrDefault();
        }

        private TreeEntry[] Hgm_GetMtlbins(IEnumerable<string> mtlbinFileNames)
        {
            var mtlbins = FindByNames(mtlbinFileNames);
            if (mtlbins == null) return null;

            UnpackFiles(mtlbins);

            return mtlbins;
        }

        private bool Hgm_ParseHgm(ref HgmDefinition def, bool showErrorDialog = true)
        {
            try
            {
                def.Hgm = Hgm.FromFile(def.HgmFilePath);
                def.Hgm._read();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($@"[Error] Failed to parse hgm: ""{def.HgmFilePath}""");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);

                if (showErrorDialog)
                {
                    var hgm = ex.Data.Contains("hgm") ? (ex.Data["hgm"] as Hgm) : null;
                    var version = hgm?.Header?.Version;
                    string jstr = null;
                    if (hgm is not null)
                    {
                        jstr = JsonConvert_Kaitai.SerializeObject(hgm, Formatting.Indented);
                    }
                    var str = Logging.GetXmlSerializedException(ex);
                    var tmb = new TextMessageBox { Title = "Error", ShortMessage = $"Failed to load hgm.\r\n{(version is not null ? $"Version: {version} (>= 12 supported)\r\n" : "")}Path: {def.HgmFilePath}", Message = str + (jstr is not null ? "\r\n\r\n" + jstr : "") };
                    tmb.ShowDialog();
                }

                return false;
            }

            return true;
        }

        private (bool success, Sltm mtlbin) Hgm_ParseMtlbin(string mtlbinFilePath)
        {
            if (!File.Exists(mtlbinFilePath)) return (false, null);

            try
            {
                return (true, Sltm.FromFile(mtlbinFilePath));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($@"[Error] Failed to parse mtlbin: ""{mtlbinFilePath}""");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);

                var str = Logging.GetXmlSerializedException(ex);
                var tmb = new TextMessageBox { Title = "Error", ShortMessage = $"Failed to load mtlbin: {mtlbinFilePath}", Message = str };
                tmb.ShowDialog();

                return (false, null);
            }
        }

        private void Hgm_GetTextures(IEnumerable<HgmMaterialDefinition> materials)
        {
            if (materials == null) return;

            var unpack = new List<(SltmTextureType typ, HgmMaterialDefinition def, TreeEntry entry)>();
            foreach (var def in materials)
            {
                var textures = new Dictionary<SltmTextureType, string>();
                if (def.Mtlbin.Mtl is Version0Struct m0)
                {
                    if (m0.Textures.BaseMap.Data.Len != 0) textures.Add(SltmTextureType.BaseMap, m0.Textures.BaseMap.Data.Str);
                    if (m0.Textures.RmMap.Data.Len != 0) textures.Add(SltmTextureType.RMMap, m0.Textures.RmMap.Data.Str);
                    if (m0.Textures.NormalMap.Data.Len != 0) textures.Add(SltmTextureType.NormalMap, m0.Textures.NormalMap.Data.Str);
                    if (m0.Textures.AoMap.Data.Len != 0) textures.Add(SltmTextureType.AOMap, m0.Textures.AoMap.Data.Str);
                    if (m0.Textures.SiMap.Data.Len != 0) textures.Add(SltmTextureType.SIMap, m0.Textures.SiMap.Data.Str);
                    if (m0.Textures.ColorizationMap.Data.Len != 0) textures.Add(SltmTextureType.ColorizationMap, m0.Textures.ColorizationMap.Data.Str);
                    if (m0.Textures.SpecialMap.Data.Len != 0) textures.Add(SltmTextureType.SpecialMap, m0.Textures.SpecialMap.Data.Str);
                }
                else if (def.Mtlbin.Mtl is Version1Struct m1)
                {
                    if (m1.Textures.BaseMap.RecType == 2) textures.Add(SltmTextureType.BaseMap, m1.Textures.BaseMap.Data.Str);
                    if (m1.Textures.RmMap.RecType == 2) textures.Add(SltmTextureType.RMMap, m1.Textures.RmMap.Data.Str);
                    if (m1.Textures.NormalMap.RecType == 2) textures.Add(SltmTextureType.NormalMap, m1.Textures.NormalMap.Data.Str);
                    if (m1.Textures.AoMap.RecType == 2) textures.Add(SltmTextureType.AOMap, m1.Textures.AoMap.Data.Str);
                    if (m1.Textures.SiMap.RecType == 2) textures.Add(SltmTextureType.SIMap, m1.Textures.SiMap.Data.Str);
                    if (m1.Textures.ColorizationMap.RecType == 2) textures.Add(SltmTextureType.ColorizationMap, m1.Textures.ColorizationMap.Data.Str);
                    if (m1.Textures.SpecialMap.RecType == 2) textures.Add(SltmTextureType.SpecialMap, m1.Textures.SpecialMap.Data.Str);
                }
                
                foreach (var (typ, textureRelFilePath) in textures)
                {
                    var match = FindByFilePath(textureRelFilePath);
                    if (match == null) continue;

                    unpack.Add((typ, def, match));
                }
            }

            var grouped = unpack.GroupBy(x => x.entry.PackFilePath).ToArray();
            foreach (var group in grouped)
            {
                UnpackFiles(group.Key, group.Select(x => x.entry));

                var i = 0;
                foreach (var tex in group)
                {
                    if (tex.entry.UnpackedFile == null) continue;

                    tex.def.TextureFilePaths.Add(tex.typ, new HgmTextureDefinition { Typ = tex.typ, FileName = tex.entry.Name, FilePath = tex.entry.UnpackedFile.FilePath });
                    i++;
                }
            }
        }

        private void Hgm_ConvertTextures(IEnumerable<HgmMaterialDefinition> materials)
        {
            materials.SelectMany(x => x.TextureFilePaths).AsParallel().ForAll(x =>
            {
                var tex = x.Value;
                var filePath = tex.FilePath;
                tex.FilePath = Path.ChangeExtension(tex.FilePath, ".png");
                Helpers.ConvertDDS(filePath, tex.FilePath);
            });
        }

        private static List<string> GetPackList(string fileName)
        {
            var p = new Process();
            p.StartInfo.FileName = "hpk.exe";
            p.StartInfo.Arguments = $@"list ""{fileName}""";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            var list = new List<string>();
            p.OutputDataReceived += (sender, eventArgs) =>
            {
                if (eventArgs.Data != null) list.Add(eventArgs.Data);
            };

            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();
            return list;
        }

        public List<string> UnpackFiles(string packFilePath, string[] filePaths)
        {
            if (!(filePaths?.Length >= 1)) return null;

            var unpackDirPath = Path.Join(_configService.Config.UnpackDirPath, Path.GetFileNameWithoutExtension(packFilePath));
            unpackDirPath = Path.GetFullPath(unpackDirPath);

            var filesStr = string.Join(" ", filePaths.Select(x => $@"""{x}"""));

            var p = new Process();
            p.StartInfo.FileName = "hpk.exe";
            p.StartInfo.Arguments = $@"extract --force ""{packFilePath}"" ""{unpackDirPath}"" {filesStr}";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            var list = new List<string>();
            p.OutputDataReceived += (sender, eventArgs) =>
            {
                if (eventArgs.Data != null) list.Add(eventArgs.Data);
            };

            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();

            var outFilePaths = filePaths.Select(x => Path.Combine(unpackDirPath, x)).ToList();

            return outFilePaths;
        }

        public void UnpackFiles(string packFilePath, IEnumerable<TreeEntry> files)
        {
            if (files?.Any() != true) return;

            var unpackDirPath = Path.Join(_configService.Config.UnpackDirPath, Path.GetFileNameWithoutExtension(packFilePath));
            unpackDirPath = Path.GetFullPath(unpackDirPath);

            var filesStr = string.Join(" ", files.Select(x => $@"""{x.FilePath}"""));

            var p = new Process();
            p.StartInfo.FileName = "hpk.exe";
            p.StartInfo.Arguments = $@"extract --force ""{packFilePath}"" ""{unpackDirPath}"" {filesStr}";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            var list = new List<string>();
            p.OutputDataReceived += (sender, eventArgs) =>
            {
                if (eventArgs.Data != null) list.Add(eventArgs.Data);
            };

            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();

            foreach (var file in files)
            {
                var unpackedFilePath = Path.Combine(unpackDirPath, file.FilePath);
                if (!File.Exists(unpackedFilePath)) continue;

                file.UnpackedFile = new FileDetails(unpackedFilePath);
            }
        }

        public void UnpackFiles(TreeEntry[] treeEntries)
        {
            var packs = treeEntries.GroupBy(x => x.PackFilePath).ToArray();
            var extracted = new Dictionary<TreeEntry, string>();
            foreach (var pack in packs)
            {
                UnpackFiles(pack.Key, pack);
            }
        }

        public void LoadDir(string dirPath)
        {
            var flat = new List<TreeEntry>();
            var list = new List<TreeEntry>();

            // use packs sub-dir if it exists
            var gameDirPath = dirPath;
            var packsDirPath = Path.Combine(gameDirPath, "Packs");
            if (Directory.Exists(packsDirPath))
                gameDirPath = packsDirPath;

            foreach (var f in Directory.GetFiles(gameDirPath, "*.hpk", SearchOption.TopDirectoryOnly))
            {
                var root = new TreeEntry { Name = Path.GetFileName(f), IsPack = true };
                flat.Add(root);
                list.Add(root);

                var lines = GetPackList(f);
                foreach (var line in lines)
                {
                    var parent = root;
                    var segments = line.Split('\\');
                    var file = segments.Last();
                    if (segments.Length > 1)
                    {
                        foreach (var segment in segments.Take(segments.Length - 1))
                        {
                            var newParent = parent.Children.Find(x => x.Name.Equals(segment, StringComparison.OrdinalIgnoreCase));
                            if (newParent != null)
                            {
                                parent = newParent;
                            }
                            else
                            {
                                newParent = new TreeEntry { Name = segment, IsDir = true, Parent = parent };
                                flat.Add(newParent);
                                parent.Children.Add(newParent);
                                parent = newParent;
                            }
                        }
                    }

                    var newChild = new TreeEntry { Name = file, IsFile = true, FilePath = line, PackFilePath = f, Parent = parent };
                    flat.Add(newChild);
                    parent.Children.Add(newChild);
                }
            }

            _entriesFlat = flat;
            _entries = list;
            _entriesNameMap = null;
            _entriesFilePathMap = null;
        }

        // Get .hgm file name without extension (will not remove LOD numbers prepended to file-extension (ex. .1.hgm))
        private static string GetFileNameWithoutExtensionHgm(string hgmFilePath)
        {
            var fileName = Path.GetFileName(hgmFilePath);
            var ext = Path.GetExtension(fileName);

            return fileName[..^ext.Length];
        }
    }

    /// <summary>
    /// Data object for Hgm files
    /// </summary>
    public class HgmDefinition
    {
        public string HgmFilePath { get; internal set; }
        public string MtlbinFilePath { get; internal set; }
        public string HgmFileName { get; internal set; }
        public string HgmFileDirPath { get; internal set; }
        public string HgmFileName_WithoutExt { get; internal set; }
        public Hgm Hgm { get; internal set; }

        public Dictionary<int, HgmMaterialDefinition> Materials { get; set; } = new Dictionary<int, HgmMaterialDefinition>();
    }

    public class HgmMaterialDefinition
    {
        public string MtlbinFilePath { get; set; }
        public Sltm Mtlbin { get; internal set; }
        public Dictionary<SltmTextureType, HgmTextureDefinition> TextureFilePaths { get; set; } = new Dictionary<SltmTextureType, HgmTextureDefinition>();
    }

    public class HgmTextureDefinition
    {
        public SltmTextureType Typ { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }

    public enum SltmTextureType
    {
        BaseMap = 1,
        RMMap = 2,
        NormalMap = 3,
        AOMap = 4,
        SIMap = 5,
        ColorizationMap = 6,
        SpecialMap = 7
    }

    /// <summary>
    /// Pack file tree view entries used in <see cref="PackBrowserControl"/>
    /// </summary>
    public class TreeEntry
    {
        public string Name { get; set; }
        public bool IsFile { get; set; }
        public bool IsDir { get; set; }
        public bool IsPack { get; set; }
        public string FilePath { get; set; }
        public string PackFilePath { get; set; }
        public List<TreeEntry> Children { get; set; } = new List<TreeEntry>();
        public TreeEntry Parent { get; set; }
        public FileDetails UnpackedFile { get; set; }
    }

    public class FileDetails
    {
        public FileDetails(string filePath)
        {
            FilePath = filePath;
            Name = Path.GetFileNameWithoutExtension(filePath);
            Filename = Path.GetFileName(filePath);
            FileDirPath = Path.GetDirectoryName(filePath);
        }

        public string Name { get; set; }
        public string Filename { get; set; }
        public string FilePath { get; set; }
        public string FileDirPath { get; set; }
    }

    public record struct ExportAndUnpackProgress
    {
        public int Current { get; init; }
        public int Total { get; init; }
        public string Action { get; set; }
        public string FileName { get; init; }
        public string ErrorMessage { get; init; }
    }
}