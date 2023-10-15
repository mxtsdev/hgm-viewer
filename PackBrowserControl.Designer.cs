using BrightIdeasSoftware;

namespace HgmViewer
{
    partial class PackBrowserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            txtFilter = new System.Windows.Forms.TextBox();
            tlvPackBrowser = new TreeListView();
            chName = new OLVColumn();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)tlvPackBrowser).BeginInit();
            SuspendLayout();
            // 
            // txtFilter
            // 
            txtFilter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtFilter.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtFilter.Location = new System.Drawing.Point(45, 3);
            txtFilter.Name = "txtFilter";
            txtFilter.Size = new System.Drawing.Size(333, 19);
            txtFilter.TabIndex = 3;
            // 
            // tlvPackBrowser
            // 
            tlvPackBrowser.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tlvPackBrowser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { chName });
            tlvPackBrowser.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            tlvPackBrowser.FullRowSelect = true;
            tlvPackBrowser.GridLines = true;
            tlvPackBrowser.Location = new System.Drawing.Point(3, 28);
            tlvPackBrowser.Name = "tlvPackBrowser";
            tlvPackBrowser.ShowGroups = false;
            tlvPackBrowser.Size = new System.Drawing.Size(375, 448);
            tlvPackBrowser.TabIndex = 2;
            tlvPackBrowser.UseFiltering = true;
            tlvPackBrowser.View = System.Windows.Forms.View.Details;
            tlvPackBrowser.VirtualMode = true;
            tlvPackBrowser.CellRightClick += tlvPackBrowser_CellRightClick;
            tlvPackBrowser.ItemActivate += tlvPackBrowser_ItemActivate;
            // 
            // chName
            // 
            chName.AspectName = "Name";
            chName.FillsFreeSpace = true;
            chName.Groupable = false;
            chName.Hideable = false;
            chName.IsEditable = false;
            chName.Sortable = false;
            chName.Text = "Name";
            chName.Width = 300;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 4);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(36, 15);
            label1.TabIndex = 4;
            label1.Text = "Filter:";
            // 
            // PackBrowserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(label1);
            Controls.Add(txtFilter);
            Controls.Add(tlvPackBrowser);
            Name = "PackBrowserControl";
            Size = new System.Drawing.Size(381, 479);
            ((System.ComponentModel.ISupportInitialize)tlvPackBrowser).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtFilter;
        private TreeListView tlvPackBrowser;
        private System.Windows.Forms.Label label1;
        private OLVColumn chName;
    }
}
