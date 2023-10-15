using Autofac;
using HgmViewer.Service;
using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Workspace;
using System;
using System.IO;
using System.Windows.Forms;
using static HgmViewer.ModelViewerControl;
using static HgmViewer.PackBrowserControl;
using static HgmViewer.SetupControl;

namespace HgmViewer
{
    /// <summary>
    /// Hgm Viewer Main Application Form
    /// </summary>
    public partial class HgmViewerForm : Form
    {
        private readonly ILifetimeScope _scope;
        private readonly ApplicationService _applicationService;
        private readonly ConfigService _configService;
        private readonly PackService _packService;
        private readonly ExportService _exportService;
        private readonly ViewerService _viewerService;

        private KryptonDockingWorkspace _workspace;
        private SetupPage _setupPage;
        private PackBrowserPage _packBrowserPage;
        private ModelViewerPage _modelViewerPage;

        private byte[] _kryptonDockingManagerConfigSaved;
        private byte[] _kryptonDockableWorkspaceLayoutSaved;
        private bool _workspaceUiSetupDone = false;

        private bool _disposedValue;

        public HgmViewerForm(
            ILifetimeScope scope,
            ApplicationService applicationService,
            ConfigService configService,
            PackService packService,
            ExportService exportService,
            ViewerService viewerService)
        {
            InitializeComponent();

            _scope = scope;
            _applicationService = applicationService;
            _configService = configService;
            _packService = packService;
            _exportService = exportService;
            _viewerService = viewerService;

            _applicationService.Setup(this);

            Load += HgmViewerForm_Load;
        }

        private async void HgmViewerForm_Load(object sender, EventArgs e)
        {
            // force load config
            _configService.LoadConfig();
            var configIsValid = _configService.IsValid();

            _workspace = dockingManager.ManageWorkspace("Workspace", kryptonDockableWorkspace1);
            dockingManager.ManageControl("Control", kryptonPanel, _workspace);
            dockingManager.ManageFloating("Floating", this);

            _setupPage = _scope.Resolve<SetupPage>();
            _setupPage.UniqueName = "Config";
            _setupPage.TextTitle = "Config";
            _setupPage.Text = "Config";
            _setupPage.ClearFlags(KryptonPageFlags.DockingAllowClose);
            _workspace.Append(_setupPage);

            if (!configIsValid)
            {
                ToggleMenuItems(false);

                return;
            }

            SetupUi();
        }

        private void ToggleMenuItems(bool enabled)
        {
            resetToolStripMenuItem.Enabled = enabled;
            openGameDirToolStripMenuItem.Enabled = enabled;
            openHgmToolStripMenuItem.Enabled = enabled;
        }

        internal void SetupUi()
        {
            if (_workspaceUiSetupDone) return;

            _workspaceUiSetupDone = true;

            ToggleMenuItems(true);

            _packBrowserPage = _scope.Resolve<PackBrowserPage>();
            _packBrowserPage.UniqueName = "PackBrowser";
            _packBrowserPage.TextTitle = "Pack Browser";
            _packBrowserPage.ClearFlags(KryptonPageFlags.DockingAllowClose);

            _modelViewerPage = _scope.Resolve<ModelViewerPage>();
            _modelViewerPage.UniqueName = "ModelViewer";
            _modelViewerPage.TextTitle = "Model Viewer";
            _modelViewerPage.Text = "Model Viewer";
            _modelViewerPage.ClearFlags(KryptonPageFlags.DockingAllowClose);

            var cell = _workspace.CellForPage(_setupPage.UniqueName);
            _workspace.CellInsert(cell, 0, _modelViewerPage);
            cell.SelectedPage = _modelViewerPage;

            var dockspaceLeft = dockingManager.AddDockspace("Control", DockingEdge.Left, new[] { _packBrowserPage });
            dockspaceLeft.DockspaceControl.Width = 300;

            _kryptonDockingManagerConfigSaved = dockingManager.SaveConfigToArray();
            _kryptonDockableWorkspaceLayoutSaved = kryptonDockableWorkspace1.SaveLayoutToArray();
        }

        private void kryptonWorkspace_WorkspaceCellAdding(object sender, WorkspaceCellEventArgs e)
        {
            e.Cell.Button.CloseButtonDisplay = ButtonDisplay.Hide;

            e.Cell.Button.CloseButtonAction = CloseButtonAction.HidePage;
        }

        private void openHgmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = "hgm",
                Filter = "Hgm (*.hgm)|*.hgm|All files (*.*)|*.*"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var unpacked = _packService.UnpackHgm(ofd.FileName);
                if (unpacked == null) return;

                var exported = _exportService.ExportGlb(unpacked);
                _viewerService.LoadGlb(exported);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dockingManager.LoadConfigFromArray(_kryptonDockingManagerConfigSaved);
            kryptonDockableWorkspace1.LoadLayoutFromArray(_kryptonDockableWorkspaceLayoutSaved);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (!Directory.Exists(dlg.SelectedPath)) return;

                _packBrowserPage.Ctrl.LoadDir(dlg.SelectedPath);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _applicationService?.Cleanup();

                    components?.Dispose();
                    components = null;


                    base.Dispose(disposing);
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}