using Autofac;
using BrightIdeasSoftware;
using HgmViewer.Classes;
using HgmViewer.Service;
using Krypton.Navigator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HgmViewer
{
    /// <summary>
    /// Browser control for Pack-files (.hpk)
    /// </summary>
    public partial class PackBrowserControl : UserControl
    {
        private readonly ApplicationService _applicationService;
        private readonly ConfigService _configService;
        private readonly PackService _packService;
        private readonly ExportService _exportService;
        private readonly ViewerService _viewerService;

        public PackBrowserControl(
            ApplicationService applicationService,
            ConfigService configService,
            PackService packService,
            ExportService exportService,
            ViewerService viewerService
            )
        {
            InitializeComponent();

            _applicationService = applicationService;
            _configService = configService;
            _packService = packService;
            _exportService = exportService;
            _viewerService = viewerService;

            Load += PackBrowserControl_Load;
        }

        private void PackBrowserControl_Load(object sender, EventArgs e)
        {
            txtFilter.KeyDown += TxtFilter_KeyDown;

            tlvPackBrowser.CanExpandGetter = (object x) => x is TreeEntry te && te.Children.Count > 0;

            tlvPackBrowser.ChildrenGetter = (object x) =>
            {
                var te = x as TreeEntry;
                return te.Children;
            };

            LoadDir(_configService.Config.GameDirPath);
        }

        private void TxtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtFilter.Text.Length > 0)
                {
                    var filter = txtFilter.Text;
                    tlvPackBrowser.ExpandAll();
                    tlvPackBrowser.ModelFilter = new ModelFilter((object x) =>
                    {
                        var te = x as TreeEntry;
                        return te.Name.Contains(filter, StringComparison.OrdinalIgnoreCase);
                    });
                }
                else
                {
                    tlvPackBrowser.ModelFilter = null;
                    tlvPackBrowser.CollapseAll();
                }
            }
        }

        private void tlvPackBrowser_ItemActivate(object sender, EventArgs e)
        {
            if (tlvPackBrowser.SelectedObject is TreeEntry item && item.IsFile && Path.GetExtension(item.FilePath).Equals(".hgm", StringComparison.OrdinalIgnoreCase))
            {
                var unpacked = _packService.UnpackHgm(item, unpackMaterialAndTextures: true);
                if (unpacked == null) return;

                var exported = _exportService.ExportGlb(unpacked);
                _viewerService.LoadGlb(exported);
            }
        }

        private void tlvPackBrowser_CellRightClick(object sender, CellRightClickEventArgs e)
        {
            var selectedFiles = tlvPackBrowser.SelectedObjects.OfType<TreeEntry>().Where(x => x.IsFile).ToArray();

            var menu = new ContextMenuStrip();
            if (selectedFiles.Length > 0)
            {
                menu.Items.Add("Extract selected file(s)", null, async (e, s) =>
                {
                    var extracted = await Task.Run(() =>
                    {
                        var packs = selectedFiles.GroupBy(x => x.PackFilePath).ToArray();
                        var extracted = new List<string>();
                        foreach (var pack in packs)
                        {
                            _applicationService.SetStatusBarText($"Extracting {pack.Count()} file(s) from {Path.GetFileName(pack.Key)}");
                            var unpackedFilePaths = _packService.UnpackFiles(pack.Key, pack.Select(x => x.FilePath).ToArray());
                            extracted.AddRange(unpackedFilePaths);
                        }
                        _applicationService.SetStatusBarText($"Completed ({extracted.Count}/{selectedFiles.Length})");

                        return extracted;
                    });
                });
            }

            var hgms = tlvPackBrowser.SelectedObjects.OfType<TreeEntry>().Where(x => x.IsFile && Path.GetExtension(x.FilePath).Equals(".hgm", StringComparison.OrdinalIgnoreCase)).ToArray();
            if (hgms.Length > 0)
            {
                menu.Items.Add("Export selected Hgm(s)", null, async (e, s) =>
                {
                    var exported = await Task.Run(() => _packService.UnpackAndExportMultiple(hgms, new Progress<ExportAndUnpackProgress>((p) =>
                    {
                        _applicationService.SetStatusBarText(
                            $"{p.Action} ({p.Current}/{p.Total})"
                                + (p.FileName != null ? $": {p.FileName}" : "")
                                + (p.ErrorMessage != null ? $" [{p.ErrorMessage}]" : "")
                            );
                    })));
                });

                menu.Items.Add("Export selected Hgm(s) + material/textures", null, async (e, s) =>
                {
                    var exported = await Task.Run(() => _packService.UnpackAndExportMultiple(hgms, new Progress<ExportAndUnpackProgress>((p) =>
                    {
                        _applicationService.SetStatusBarText(
                            $"{p.Action} ({p.Current}/{p.Total})"
                                + (p.FileName != null ? $": {p.FileName}" : "")
                                + (p.ErrorMessage != null ? $" [{p.ErrorMessage}]" : "")
                            );
                    }), unpackMaterialAndTextures: true));
                });
            }

            var dds = tlvPackBrowser.SelectedObjects.OfType<TreeEntry>().Where(x => x.IsFile && Path.GetExtension(x.FilePath).Equals(".dds", StringComparison.OrdinalIgnoreCase)).ToArray();
            if (dds.Length > 0)
            {
                menu.Items.Add("Extract selected dds(s) + convert to .tga", null, async (e, s) =>
                {
                    var extracted = await Task.Run(() =>
                    {
                        var packs = dds.GroupBy(x => x.PackFilePath).ToArray();
                        var extracted = new List<string>();
                        foreach (var pack in packs)
                        {
                            _applicationService.SetStatusBarText($"Extracting {pack.Count()} file(s) from {Path.GetFileName(pack.Key)}");
                            var unpackedFilePaths = _packService.UnpackFiles(pack.Key, pack.Select(x => x.FilePath).ToArray());

                            foreach (var unpackedFilePath in unpackedFilePaths)
                            {
                                Helpers.ConvertDDS(unpackedFilePath, Path.ChangeExtension(unpackedFilePath, ".tga"));
                            }

                            extracted.AddRange(unpackedFilePaths);
                        }
                        _applicationService.SetStatusBarText($"Completed ({extracted.Count}/{dds.Length})");

                        return extracted;
                    });
                });
            }

            if (menu.Items.Count == 0) return;

            e.MenuStrip = menu;
        }

        internal void LoadDir(string dirPath)
        {
            _packService.LoadDir(dirPath);
            tlvPackBrowser.SetObjects(_packService.Entries);
        }

        /// <summary>
        /// Krypton Docking page for <see cref="PackBrowserControl"/>
        /// </summary>
        public class PackBrowserPage : KryptonPage
        {
            private readonly ILifetimeScope _scope;
            private readonly PackBrowserControl _ctrl;

            public PackBrowserControl Ctrl => _ctrl;

            public PackBrowserPage(ILifetimeScope scope)
            {
                _scope = scope;

                _ctrl = scope.Resolve<PackBrowserControl>();

                _ctrl.Dock = DockStyle.Fill;
                Controls.Add(_ctrl);
            }
        }
    }
}