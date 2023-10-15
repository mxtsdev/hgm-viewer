namespace HgmViewer
{
    partial class SetupControl
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
            kpgConfig = new Krypton.Toolkit.KryptonPropertyGrid();
            SuspendLayout();
            // 
            // kpgConfig
            // 
            kpgConfig.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            kpgConfig.CategoryForeColor = System.Drawing.Color.FromArgb(30, 57, 91);
            kpgConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            kpgConfig.HelpBackColor = System.Drawing.Color.FromArgb(187, 206, 230);
            kpgConfig.HelpForeColor = System.Drawing.Color.FromArgb(30, 57, 91);
            kpgConfig.LineColor = System.Drawing.Color.FromArgb(179, 196, 216);
            kpgConfig.Location = new System.Drawing.Point(3, 3);
            kpgConfig.Name = "kpgConfig";
            kpgConfig.Size = new System.Drawing.Size(341, 332);
            kpgConfig.TabIndex = 0;
            // 
            // SetupControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(kpgConfig);
            Name = "SetupControl";
            Size = new System.Drawing.Size(347, 338);
            ResumeLayout(false);
        }

        #endregion

        private Krypton.Toolkit.KryptonPropertyGrid kpgConfig;
    }
}
