using System;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace HgmViewer.Service
{
    /// <summary>
    /// Application Service
    /// </summary>
    public class ApplicationService : IDisposable
    {
        private HgmViewerForm _hgmViewerForm;

        private Timer _timer;
        private DateTime? _clearAt = null;

        private bool _disposedValue;

        public ApplicationService()
        {
            _timer = new Timer((state) =>
            {
                if (_clearAt.HasValue && _clearAt.Value <= DateTime.Now)
                {
                    SetStatusBarTextInternal("");
                    _clearAt = null;
                }
            }, null, 50, 50);
        }

        public void Setup(HgmViewerForm hgmViewerForm)
        {
            _hgmViewerForm = hgmViewerForm;
        }

        public void SetupUi()
        {
            if (_hgmViewerForm.InvokeRequired)
            {
                _hgmViewerForm.BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                {
                    SetupUi();
                }));

                return;
            }

            _hgmViewerForm.SetupUi();
        }

        public static async Task<bool> DownloadHpkIfMissing()
        {
            if (File.Exists("hpk.exe")) return true;

            try
            {
                using var wc = new HttpClient();
                wc.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("HgmViewerBot", "1.0"));
                var str = await wc.GetStringAsync("https://api.github.com/repos/nickelc/hpk/releases/latest");
                var list = JsonConvert.DeserializeAnonymousType(str, new { assets = new[] { new { name = "", browser_download_url = "" } } });

                var win_asset = list?.assets?.FirstOrDefault(x => x?.name.Contains("windows", StringComparison.OrdinalIgnoreCase) == true);
                if (win_asset == null) return false;

                using var stream = await wc.GetStreamAsync(win_asset.browser_download_url);
                using var archive = new ZipArchive(stream, ZipArchiveMode.Read, true);

                var entry = archive.Entries.FirstOrDefault(x => x.Name.Equals("hpk.exe", StringComparison.OrdinalIgnoreCase));
                if (entry == null) return false;

                entry.ExtractToFile("hpk.exe");

                return true;
            }
            catch { }

            return false;
        }

        public void SetStatusBarText(string txt, int clearAfterMs = 2000)
        {
            _clearAt = DateTime.Now.AddMilliseconds(clearAfterMs);

            SetStatusBarTextInternal(txt);
        }

        private void SetStatusBarTextInternal(string txt)
        {
            _hgmViewerForm.Invoke(() => _hgmViewerForm.tsStatusBar.Text = txt);
        }

        public void Cleanup()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _clearAt = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _timer?.Dispose();
                    _timer = null;
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
