namespace HgmViewer
{
    partial class TextMessageBox
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tbMessage = new System.Windows.Forms.TextBox();
            tbShortMessage = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // tbMessage
            // 
            tbMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tbMessage.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            tbMessage.Location = new System.Drawing.Point(12, 50);
            tbMessage.Multiline = true;
            tbMessage.Name = "tbMessage";
            tbMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            tbMessage.Size = new System.Drawing.Size(444, 257);
            tbMessage.TabIndex = 0;
            tbMessage.WordWrap = false;
            // 
            // tbShortMessage
            // 
            tbShortMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tbShortMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            tbShortMessage.Enabled = false;
            tbShortMessage.Location = new System.Drawing.Point(12, 12);
            tbShortMessage.Multiline = true;
            tbShortMessage.Name = "tbShortMessage";
            tbShortMessage.ReadOnly = true;
            tbShortMessage.Size = new System.Drawing.Size(444, 32);
            tbShortMessage.TabIndex = 1;
            // 
            // TextMessageBox
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(468, 319);
            Controls.Add(tbShortMessage);
            Controls.Add(tbMessage);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TextMessageBox";
            Text = "Message";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.TextBox tbShortMessage;
    }
}