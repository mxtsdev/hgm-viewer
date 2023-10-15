using System;
using System.IO;
using System.Numerics;

namespace HgmViewer.Service
{
    /// <summary>
    /// 3D-Model Viewer Service
    /// </summary>
    public class ViewerService
    {
        public delegate void ReadyEventHandler();
        public delegate void LoadModelEventHandler(LoadModelDefinition def);

        public event LoadModelEventHandler LoadModel;
        public event ReadyEventHandler Ready;

        private readonly ConfigService _configService;

        public ViewerService(ConfigService configService)
        {
            _configService = configService;
        }

        protected void OnLoadModel(LoadModelDefinition def)
        {
            LoadModel?.Invoke(def);
        }

        internal void OnReady()
        {
            if (Ready != null)
            {
                Ready.Invoke();
            }
            else
            {
                // automatically load last viewed Glb
                if (_configService.Config.LastLoadedGlb != null && File.Exists(_configService.Config.LastLoadedGlb))
                {
                    LoadGlb(_configService.Config.LastLoadedGlb);
                }
            }
        }

        public void LoadGlb(ExportedModelDefinition def)
        {
            var load = new LoadModelDefinition
            {
                ModelFileName = def.ModelFileName,
                ModelFilePath = "https://hgmviewer-exports.test/" + Path.GetRelativePath(_configService.Config.ExportDirPath, def.ModelFilePath).Replace(@"\", "/"),
                LookAt = def.LookAt,
                Dist = def.Dist
            };

            OnLoadModel(load);

            _configService.Config.LastLoadedGlb = def.ModelFilePath;
            _configService.SaveConfig();
        }

        public void LoadGlb(string filePath)
        {
            var load = new LoadModelDefinition
            {
                ModelFileName = Path.GetFileName(filePath),
                ModelFilePath = "https://hgmviewer-exports.test/" + Path.GetRelativePath(_configService.Config.ExportDirPath, filePath).Replace(@"\", "/")
            };

            OnLoadModel(load);
        }
    }

    /// <summary>
    /// Data object for loading a model in three.js 3D-viewer
    /// </summary>
    public class LoadModelDefinition
    {
        public string ModelFileName { get; set; }
        public string ModelFilePath { get; set; }
        public string ModelDirPath { get; set; }
        public Vector3? LookAt { get; set; }
        public float? Dist { get; set; }
    }
}