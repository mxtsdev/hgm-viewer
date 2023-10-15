using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using HgmViewer.Classes;
using HgmViewer.Service;
using Krypton.Navigator;

namespace HgmViewer
{
    public partial class SetupControl : UserControl
    {
        private readonly ConfigService _configService;
        private readonly ApplicationService _applicationService;

        public SetupControl(
            ConfigService configService,
            ApplicationService applicationService)
        {
            InitializeComponent();

            _configService = configService;
            _applicationService = applicationService;

            Load += SetupControl_Load;
        }

        private async void SetupControl_Load(object sender, EventArgs e)
        {
            kpgConfig.SelectedObject = new ConfigProxy(_configService.Config, () =>
            {
                if (_configService.IsValid())
                {
                    _configService.SaveConfig();
                    _applicationService.SetupUi();
                }
            });

            await Task.Delay(2500);
            var downloaded = await Task.Run(async () =>
            {
                return await QueryUserDownloadHpkExeIfMissing();
            });

            if (downloaded)
                kpgConfig.Refresh();
        }

        private async Task<bool> QueryUserDownloadHpkExeIfMissing()
        {
            if (_configService.Config.HpkExeFilePath == null || !File.Exists(_configService.Config.HpkExeFilePath))
            {
                if (MessageBox.Show("Hpk Archiver (hpk.exe) is required to unpack game assets from .hpk archives. Would you like to download it now?", "Automatic download", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    var hpk = await ApplicationService.DownloadHpkIfMissing();
                    if (hpk)
                    {
                        var configProxy = kpgConfig.SelectedObject as ConfigProxy;
                        configProxy.HpkExeFilePath = "hpk.exe";
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Act as proxy for a config item to control the exposed properties to the property grid.
        /// </summary>
        protected class ConfigProxy
        {
            private readonly Config _item;
            private readonly Action _revalidate;

            public ConfigProxy(Config item, Action revalidate)
            {
                _item = item;
                _revalidate = revalidate;
            }

            [Category(@"Config")]
            [DisplayName("HPK Archiver (hpk.exe)")]
            [Description("Used to unpack .hpk archives.\n\nDownload from:\nhttps://github.com/nickelc/hpk/releases/latest")]
            [Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(UITypeEditor))]
            public string HpkExeFilePath
            {
                get => _item.HpkExeFilePath;
                set
                {
                    if (!File.Exists(value)) return;

                    _item.HpkExeFilePath = value;
                    _revalidate();
                }
            }

            [Category(@"Config")]
            [DisplayName("Game directory")]
            [Editor(typeof(FolderNameEditorEx), typeof(UITypeEditor))]
            public string GameDirPath
            {
                get => _item.GameDirPath;
                set
                {
                    if (!Directory.Exists(value)) return;

                    _item.GameDirPath = value;
                    _revalidate();
                }
            }

            [Category(@"Config")]
            [DisplayName("Unpacked files directory")]
            [Editor(typeof(FolderNameEditorEx), typeof(UITypeEditor))]
            public string UnpackDirPath
            {
                get => _item.UnpackDirPath;
                set
                {
                    if (!Directory.Exists(value)) return;

                    _item.UnpackDirPath = value;
                    _revalidate();
                }
            }

            [Category(@"Config")]
            [DisplayName("Exported files directory")]
            [Editor(typeof(FolderNameEditorEx), typeof(UITypeEditor))]
            public string ExportDirPath
            {
                get => _item.ExportDirPath;
                set
                {
                    if (!Directory.Exists(value)) return;

                    _item.ExportDirPath = value;
                    _revalidate();
                }
            }
        }

        /// <summary>
        /// Krypton Docking page for <see cref="SetupControl"/>
        /// </summary>
        public class SetupPage : KryptonPage
        {
            private readonly ILifetimeScope _scope;
            private readonly SetupControl _ctrl;

            public SetupControl Ctrl => _ctrl;

            public SetupPage(ILifetimeScope scope)
            {
                _scope = scope;

                _ctrl = scope.Resolve<SetupControl>();

                _ctrl.Dock = DockStyle.Fill;
                Controls.Add(_ctrl);
            }
        }
    }
}
