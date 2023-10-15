using Autofac;
using HgmViewer.Classes;
using HgmViewer.Service;
using Krypton.Navigator;
using Microsoft.Web.WebView2.Core;
using System;
using System.Windows.Forms;

namespace HgmViewer
{
    /// <summary>
    /// 3D-Model Preview Control
    /// </summary>
    public partial class ModelViewerControl : UserControl
    {
        private readonly ILifetimeScope _scope;
        internal readonly WebViewContext _webViewContext;
        private const bool _acceleratorKeysEnabled = false;
        private const bool _webViewNavigationBarDisabled = true;

        public ModelViewerControl(ILifetimeScope scope)
        {
            InitializeComponent();

            _scope = scope;
            _webViewContext = _scope.Resolve<WebViewContext>(new NamedParameter("modelViewerControl", this));

            if (_webViewNavigationBarDisabled)
            {
                splitContainer.Panel1Collapsed = true;
            }

            btnGo.Text = "\uE017";

            AttachControlEventHandlers(webView2Control);

            Load += ModelViewerControl_Load;
        }

        private void ModelViewerControl_Load(object sender, EventArgs e)
        {
            webView2Control.Dock = DockStyle.Fill;
        }

        private void AddressBarNavigate()
        {
            var rawUrl = txtUrl.Text;
            Uri uri;
            if (Uri.IsWellFormedUriString(rawUrl, UriKind.Absolute))
            {
                uri = new Uri(rawUrl);
            }
            else if (!rawUrl.Contains(' ') && rawUrl.Contains('.'))
            {
                uri = new Uri("http://" + rawUrl);
            }
            else
            {
                uri = new Uri("https://www.google.com/search?q=" +
                    String.Join("+", Uri.EscapeDataString(rawUrl).Split(new string[] { "%20" }, StringSplitOptions.RemoveEmptyEntries)));
            }

            webView2Control.Source = uri;
        }

        private void AttachControlEventHandlers(Microsoft.Web.WebView2.WinForms.WebView2 control)
        {
            control.CoreWebView2InitializationCompleted += WebView2Control_CoreWebView2InitializationCompleted;
            control.NavigationCompleted += WebView2Control_NavigationCompleted;
            control.SourceChanged += CoreWebView2_SourceChanged;
            control.KeyDown += WebView2Control_KeyDown;
            control.KeyUp += WebView2Control_KeyUp;
        }

        private void WebView2Control_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                MessageBox.Show($"WebView2 creation failed with exception = {e.InitializationException}");
                return;
            }

            webView2Control.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            webView2Control.CoreWebView2.SourceChanged += CoreWebView2_SourceChanged;
            webView2Control.CoreWebView2.HistoryChanged += CoreWebView2_HistoryChanged;

            _webViewContext.InitializationCompleted();
        }

        private void WebView2Control_KeyUp(object sender, KeyEventArgs e)
        {
            if (ConfigService.IsDebug && e.KeyCode == Keys.F12)
                return;

            if (!_acceleratorKeysEnabled)
                e.Handled = true;
        }

        private void WebView2Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (ConfigService.IsDebug && e.KeyCode == Keys.F12)
                return;

            if (!_acceleratorKeysEnabled)
                e.Handled = true;
        }

        private async void WebView2Control_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            _webViewContext.NavigationCompleted(e);

            await webView2Control.CoreWebView2.Profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.BrowsingHistory);
        }

        private void CoreWebView2_HistoryChanged(object sender, object e)
        {
            btnBack.Enabled = webView2Control.CoreWebView2.CanGoBack;
            btnForward.Enabled = webView2Control.CoreWebView2.CanGoForward;
        }

        private void CoreWebView2_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {
            txtUrl.Text = webView2Control.Source.AbsoluteUri;
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            webView2Control.Reload();
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            AddressBarNavigate();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            webView2Control.GoBack();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            webView2Control.GoForward();
        }

        private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                AddressBarNavigate();
                e.Handled = true;
            }
        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
            if (txtUrl.Text.IndexOf('\r') != -1 || txtUrl.Text.IndexOf('\n') != -1)
            {
                txtUrl.Text = txtUrl.Text.Replace("\r", "").Replace("\n", "");
            }
        }

        /// <summary>
        /// Krypton Docking page for <see cref="ModelViewerControl"/>
        /// </summary>
        public class ModelViewerPage : KryptonPage
        {
            private readonly ILifetimeScope _scope;

            private readonly ModelViewerControl _ctrl;

            public ModelViewerControl Ctrl => _ctrl;

            public ModelViewerPage(ILifetimeScope scope)
            {
                _scope = scope;

                _ctrl = scope.Resolve<ModelViewerControl>();

                _ctrl.Dock = DockStyle.Fill;
                Controls.Add(_ctrl);
            }
        }
    }
}