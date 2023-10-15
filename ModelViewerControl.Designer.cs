namespace HgmViewer
{
    partial class ModelViewerControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer = new System.Windows.Forms.SplitContainer();
            btnGo = new System.Windows.Forms.Button();
            txtUrl = new System.Windows.Forms.TextBox();
            btnStop = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            btnForward = new System.Windows.Forms.Button();
            btnBack = new System.Windows.Forms.Button();
            webView2Control = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView2Control).BeginInit();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer.IsSplitterFixed = true;
            splitContainer.Location = new System.Drawing.Point(3, 3);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(btnGo);
            splitContainer.Panel1.Controls.Add(txtUrl);
            splitContainer.Panel1.Controls.Add(btnStop);
            splitContainer.Panel1.Controls.Add(btnRefresh);
            splitContainer.Panel1.Controls.Add(btnForward);
            splitContainer.Panel1.Controls.Add(btnBack);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(webView2Control);
            splitContainer.Size = new System.Drawing.Size(1086, 558);
            splitContainer.SplitterDistance = 45;
            splitContainer.SplitterWidth = 1;
            splitContainer.TabIndex = 0;
            // 
            // btnGo
            // 
            btnGo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnGo.Font = new System.Drawing.Font("Segoe Fluent Icons", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnGo.Location = new System.Drawing.Point(785, 4);
            btnGo.Margin = new System.Windows.Forms.Padding(2);
            btnGo.Name = "btnGo";
            btnGo.Size = new System.Drawing.Size(37, 37);
            btnGo.TabIndex = 11;
            btnGo.Text = ">";
            btnGo.UseVisualStyleBackColor = true;
            btnGo.Click += BtnGo_Click;
            // 
            // txtUrl
            // 
            txtUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtUrl.Location = new System.Drawing.Point(171, 4);
            txtUrl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtUrl.Multiline = true;
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new System.Drawing.Size(607, 34);
            txtUrl.TabIndex = 10;
            txtUrl.Text = "about:blank";
            txtUrl.WordWrap = false;
            txtUrl.TextChanged += txtUrl_TextChanged;
            txtUrl.KeyDown += txtUrl_KeyDown;
            // 
            // btnStop
            // 
            btnStop.Font = new System.Drawing.Font("Segoe Fluent Icons", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnStop.Location = new System.Drawing.Point(128, 2);
            btnStop.Margin = new System.Windows.Forms.Padding(2);
            btnStop.Name = "btnStop";
            btnStop.Size = new System.Drawing.Size(37, 37);
            btnStop.TabIndex = 9;
            btnStop.Text = "";
            btnStop.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            btnRefresh.Font = new System.Drawing.Font("Segoe Fluent Icons", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnRefresh.Location = new System.Drawing.Point(86, 2);
            btnRefresh.Margin = new System.Windows.Forms.Padding(2);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(37, 37);
            btnRefresh.TabIndex = 8;
            btnRefresh.Text = "";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // btnForward
            // 
            btnForward.Enabled = false;
            btnForward.Font = new System.Drawing.Font("Segoe Fluent Icons", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnForward.Location = new System.Drawing.Point(44, 2);
            btnForward.Margin = new System.Windows.Forms.Padding(2);
            btnForward.Name = "btnForward";
            btnForward.Size = new System.Drawing.Size(37, 37);
            btnForward.TabIndex = 7;
            btnForward.Text = "";
            btnForward.UseVisualStyleBackColor = true;
            btnForward.Click += btnForward_Click;
            // 
            // btnBack
            // 
            btnBack.Enabled = false;
            btnBack.Font = new System.Drawing.Font("Segoe Fluent Icons", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnBack.Location = new System.Drawing.Point(2, 2);
            btnBack.Margin = new System.Windows.Forms.Padding(2);
            btnBack.Name = "btnBack";
            btnBack.Size = new System.Drawing.Size(37, 37);
            btnBack.TabIndex = 6;
            btnBack.Text = "";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // webView2Control
            // 
            webView2Control.AllowExternalDrop = true;
            webView2Control.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            webView2Control.CreationProperties = null;
            webView2Control.DefaultBackgroundColor = System.Drawing.Color.White;
            webView2Control.Location = new System.Drawing.Point(2, 2);
            webView2Control.Margin = new System.Windows.Forms.Padding(2);
            webView2Control.Name = "webView2Control";
            webView2Control.Size = new System.Drawing.Size(1082, 508);
            webView2Control.Source = new System.Uri("about:blank", System.UriKind.Absolute);
            webView2Control.TabIndex = 9;
            webView2Control.ZoomFactor = 1D;
            // 
            // ModelViewerControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainer);
            Name = "ModelViewerControl";
            Size = new System.Drawing.Size(1092, 564);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel1.PerformLayout();
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView2Control).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        internal System.Windows.Forms.Button btnGo;
        internal System.Windows.Forms.TextBox txtUrl;
        internal System.Windows.Forms.Button btnStop;
        internal System.Windows.Forms.Button btnRefresh;
        internal System.Windows.Forms.Button btnForward;
        internal System.Windows.Forms.Button btnBack;
        internal Microsoft.Web.WebView2.WinForms.WebView2 webView2Control;
    }
}
