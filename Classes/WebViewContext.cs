using HgmViewer.Service;
using Microsoft.Web.WebView2.Core;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HgmViewer.Classes
{
    public class WebViewContext
    {
        private readonly ModelViewerControl _modelViewerControl;
        private readonly ConfigService _configService;
        private readonly ViewerService _viewerService;
        private readonly ExportService _exportService;

        private Microsoft.Web.WebView2.WinForms.WebView2 _webView2Control => _modelViewerControl.webView2Control;

        private const string _keyhandlercode = @"
                    (function() {
                        function KeyHandler(event)
                        {
                            if (event.code == 'KeyE') {
                                ToggleAxis();
                                event.preventDefault();
                                event.stopPropagation();
                            } else if (event.code == 'KeyR') {
                                RestoreCamera();
                                event.preventDefault();
                                event.stopPropagation();
                            }
                        }
                        function DblClickHandler(event)
                        {
                            RestoreCamera();
                        }

                        document.addEventListener('keydown', KeyHandler);
                        document.addEventListener('dblclick', DblClickHandler);
                    })()
        ";

        public WebViewContext(
            ConfigService configService,
            ViewerService viewerService,
            ExportService exportService,
            ModelViewerControl modelViewerControl)
        {
            _configService = configService;
            _viewerService = viewerService;
            _exportService = exportService;
            _modelViewerControl = modelViewerControl;

            _viewerService.LoadModel += _viewerService_LoadModel;
        }

        private async void _viewerService_LoadModel(LoadModelDefinition def)
        {
            await LoadModel(def);
        }

        private async Task LoadModel(LoadModelDefinition def)
        {
            var vars = def.LookAt.HasValue ? string.Join(", ", new[] { def.LookAt?.X, def.LookAt?.Y, def.LookAt?.Z }.Select(x => string.Create(CultureInfo.InvariantCulture, $"{x}"))) : null;
            vars = vars != null ? $", [{vars}]" + (def.Dist != null ? ", " + string.Create(CultureInfo.InvariantCulture, $"{def.Dist}") : "") : "";
            await _webView2Control.CoreWebView2.ExecuteScriptAsync($@"
            (function() {{
                LoadNewModel('{def.ModelFilePath}'{vars});
            }})()");
        }

        private int _navigationCount = 0;
        internal void NavigationCompleted(CoreWebView2NavigationCompletedEventArgs e)
        {
            if (_navigationCount == 0)
            {
                var exportDirPathAbsolute = Path.GetFullPath(_configService.Config.ExportDirPath);

                _webView2Control.CoreWebView2.SetVirtualHostNameToFolderMapping("hgmviewer-exports.test", exportDirPathAbsolute, CoreWebView2HostResourceAccessKind.Allow);
                _webView2Control.CoreWebView2.SetVirtualHostNameToFolderMapping("hgmviewer.test", Path.Combine(Directory.GetCurrentDirectory(), "Web"), CoreWebView2HostResourceAccessKind.Allow);
                _webView2Control.CoreWebView2.Navigate("https://hgmviewer.test/index.htm");
            }
            else if (_navigationCount == 1 && e.IsSuccess)
            {
                _viewerService.OnReady();
            }
            _navigationCount++;
        }

        public void InitializationCompleted()
        {
            _webView2Control.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(_keyhandlercode);
        }
    }
}