using BrightIdeasSoftware;

namespace HgmViewer
{
    partial class HgmViewerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HgmViewerForm));
            kryptonPanel = new Krypton.Toolkit.KryptonPanel();
            kryptonDockableWorkspace1 = new Krypton.Docking.KryptonDockableWorkspace();
            kryptonManager = new Krypton.Toolkit.KryptonManager(components);
            dockingManager = new Krypton.Docking.KryptonDockingManager();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            constructionMaterialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openGameDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openHgmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            tsStatusBar = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)kryptonPanel).BeginInit();
            kryptonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)kryptonDockableWorkspace1).BeginInit();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // kryptonPanel
            // 
            kryptonPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            kryptonPanel.Controls.Add(kryptonDockableWorkspace1);
            kryptonPanel.Location = new System.Drawing.Point(3, 27);
            kryptonPanel.Name = "kryptonPanel";
            kryptonPanel.Size = new System.Drawing.Size(1057, 398);
            kryptonPanel.TabIndex = 2;
            // 
            // kryptonDockableWorkspace1
            // 
            kryptonDockableWorkspace1.ActivePage = null;
            kryptonDockableWorkspace1.AutoHiddenHost = false;
            kryptonDockableWorkspace1.CompactFlags = Krypton.Workspace.CompactFlags.RemoveEmptyCells | Krypton.Workspace.CompactFlags.RemoveEmptySequences | Krypton.Workspace.CompactFlags.PromoteLeafs;
            kryptonDockableWorkspace1.ContainerBackStyle = Krypton.Toolkit.PaletteBackStyle.PanelClient;
            kryptonDockableWorkspace1.Dock = System.Windows.Forms.DockStyle.Fill;
            kryptonDockableWorkspace1.Location = new System.Drawing.Point(0, 0);
            kryptonDockableWorkspace1.Name = "kryptonDockableWorkspace1";
            // 
            // 
            // 
            kryptonDockableWorkspace1.Root.UniqueName = "038e268a5454462ebb4b47ca2af8cb96";
            kryptonDockableWorkspace1.Root.WorkspaceControl = kryptonDockableWorkspace1;
            kryptonDockableWorkspace1.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.LowProfile;
            kryptonDockableWorkspace1.ShowMaximizeButton = false;
            kryptonDockableWorkspace1.Size = new System.Drawing.Size(1057, 398);
            kryptonDockableWorkspace1.SplitterWidth = 5;
            kryptonDockableWorkspace1.TabIndex = 2;
            kryptonDockableWorkspace1.TabStop = true;
            kryptonDockableWorkspace1.WorkspaceCellAdding += kryptonWorkspace_WorkspaceCellAdding;
            // 
            // menuStrip1
            // 
            menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { constructionMaterialsToolStripMenuItem, windowsToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            menuStrip1.Size = new System.Drawing.Size(1063, 24);
            menuStrip1.TabIndex = 8;
            menuStrip1.Text = "menuStrip1";
            // 
            // constructionMaterialsToolStripMenuItem
            // 
            constructionMaterialsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { openGameDirToolStripMenuItem, openHgmToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            constructionMaterialsToolStripMenuItem.Name = "constructionMaterialsToolStripMenuItem";
            constructionMaterialsToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            constructionMaterialsToolStripMenuItem.Text = "File";
            // 
            // openGameDirToolStripMenuItem
            // 
            openGameDirToolStripMenuItem.Name = "openGameDirToolStripMenuItem";
            openGameDirToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            openGameDirToolStripMenuItem.Text = "Open game directory";
            openGameDirToolStripMenuItem.Click += toolStripMenuItem2_Click;
            // 
            // openHgmToolStripMenuItem
            // 
            openHgmToolStripMenuItem.Name = "openHgmToolStripMenuItem";
            openHgmToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            openHgmToolStripMenuItem.Text = "Open hgm";
            openHgmToolStripMenuItem.Click += openHgmToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(183, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // windowsToolStripMenuItem
            // 
            windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { resetToolStripMenuItem });
            windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            windowsToolStripMenuItem.Size = new System.Drawing.Size(68, 22);
            windowsToolStripMenuItem.Text = "Windows";
            // 
            // resetToolStripMenuItem
            // 
            resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resetToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            resetToolStripMenuItem.Text = "Reset layout";
            resetToolStripMenuItem.Click += resetToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsStatusBar });
            statusStrip1.Location = new System.Drawing.Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip1.Size = new System.Drawing.Size(1063, 22);
            statusStrip1.TabIndex = 14;
            statusStrip1.Text = "statusStrip1";
            // 
            // tsStatusBar
            // 
            tsStatusBar.Name = "tsStatusBar";
            tsStatusBar.Size = new System.Drawing.Size(0, 17);
            // 
            // HgmViewerForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1063, 450);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(kryptonPanel);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MinimumSize = new System.Drawing.Size(250, 250);
            Name = "HgmViewerForm";
            Text = "HGM Viewer (for Haemimont Sol Engine games)";
            ((System.ComponentModel.ISupportInitialize)kryptonPanel).EndInit();
            kryptonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)kryptonDockableWorkspace1).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TreeListView tlvPackBrowser;
        private OLVColumn chName;
        private Krypton.Toolkit.KryptonPanel kryptonPanel;
        private Krypton.Docking.KryptonDockableWorkspace kryptonDockableWorkspace1;
        private Krypton.Toolkit.KryptonManager kryptonManager;
        private Krypton.Docking.KryptonDockingManager dockingManager;
        internal System.Windows.Forms.MenuStrip menuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem constructionMaterialsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem openHgmToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        internal System.Windows.Forms.ToolStripStatusLabel tsStatusBar;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openGameDirToolStripMenuItem;
    }
}