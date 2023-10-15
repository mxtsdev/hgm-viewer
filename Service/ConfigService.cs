using HgmViewer.Classes;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;
using static HgmViewer.PackBrowserControl;

namespace HgmViewer.Service
{
    /// <summary>
    /// Config Service
    /// </summary>
    public class ConfigService
    {
        private const string _configFilePath = "config.json";
        private Config _config;

#if DEBUG
        private const bool _isDebug = true;
#else
        private const bool _isDebug = false;
#endif

        public static bool IsDebug => _isDebug;

        public Config Config
        {
            get
            {
                if (_config == null) LoadConfig();

                return _config;
            }
        }

        public void SaveConfig()
        {
            File.WriteAllText(_configFilePath, JsonConvert.SerializeObject(_config, Formatting.Indented));
        }

        internal void LoadConfig()
        {
            if (!File.Exists(_configFilePath))
            {
                _config = new Config();
                InitConfigDefaults();
                SaveConfig();
            }
            else
            {
                _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(_configFilePath));
                if (InitConfigDefaults())
                    SaveConfig();
            }
        }

        public bool IsValid()
        {
            if (_config.UnpackDirPath == null || !Directory.Exists(_config.UnpackDirPath))
                return false;

            if (_config.ExportDirPath == null || !Directory.Exists(_config.ExportDirPath))
                return false;

            if (_config.GameDirPath == null || !Directory.Exists(_config.GameDirPath))
                return false;

            if (_config.HpkExeFilePath == null || !File.Exists(_config.HpkExeFilePath))
                return false;

            return true;
        }

        private bool InitConfigDefaults()
        {
            var changed = false;

            if (_config.UnpackDirPath == null || !Directory.Exists(_config.UnpackDirPath))
            {
                _config.UnpackDirPath = @"Temp\Unpacked";
                Directory.CreateDirectory(_config.UnpackDirPath);
                changed = true;
            }

            if (_config.ExportDirPath == null || !Directory.Exists(_config.ExportDirPath))
            {
                _config.ExportDirPath = @"Exported";
                Directory.CreateDirectory(_config.ExportDirPath);
                changed = true;
            }

            if (_config.GameDirPath == null || !Directory.Exists(_config.GameDirPath))
            {
                _config.GameDirPath = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Haemimont Games\\Jagged Alliance 3", "Path", null) as string;
                if (_config.GameDirPath == null || !Directory.Exists(_config.GameDirPath))
                {
                    var dlg = new FolderBrowserDialog { Description = "Select game installation directory", UseDescriptionForTitle = true };
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (Directory.Exists(dlg.SelectedPath))
                        {
                            _config.GameDirPath = dlg.SelectedPath;
                            changed = true;
                        }
                    }
                }
            }

            return changed;
        }
    }
}