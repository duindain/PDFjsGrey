using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace pdfTester
{
    public class PDFjsPageVM : BaseViewModel
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _pdfSource;
        public string PDFSource
        {
            get => _pdfSource;
            set => SetProperty(ref _pdfSource, value);
        }

        private string _localFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "pdfjs");

        public async Task LoadUrl(string url)
        {
            var localPath = string.Empty;

            var dependency = DependencyService.Get<ILocalFileProvider>();

            Title = Path.GetFileNameWithoutExtension(url);

            var fileName = Path.GetFileName(url);            
            localPath = Path.Combine(_localFilePath, fileName);

            if (File.Exists(localPath) == false)
            {
                System.Diagnostics.Debug.WriteLine($"PDFjsPageVM.ctor: File didnt exist locally {localPath}");
                // Download PDF locally for viewing
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var pdfStream = await httpClient.GetStreamAsync(url);
                        System.Diagnostics.Debug.WriteLine($"PDFjsPageVM.ctor: Downloaded PDF file");

                         await dependency.RequestPermissionToSavePDF(async (grantedPermissions) =>
                         {
                             if (grantedPermissions)
                             {
                                 await SaveFileToDisk(pdfStream, localPath);
                                 OpenPdf(localPath, url);
                             }
                         });
                    }
                    catch (AggregateException ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"PDFjsPageVM.ctor: Certificate error? only SSL sites are supported without additional config {ex.Message}");
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"PDFjsPageVM.ctor: File exists locally {localPath}");
                OpenPdf(localPath, url);
            }
        }

        private void OpenPdf(string localPath, string url)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                System.Diagnostics.Debug.WriteLine($"PDFjsPageVM.OpenPdf: Opening PDF {localPath}");
                PDFSource = $"file:///android_asset/pdfjs/web/viewer.html?file=file:///{WebUtility.UrlEncode(localPath)}";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"PDFjsPageVM.OpenPdf: Opening PDF {url}");
                PDFSource = url;
            }
        }

        private async Task<string> SaveFileToDisk(Stream pdfStream, string filePath)
        {
            var fullPath = Path.GetFullPath(filePath.Substring(0, filePath.IndexOf(Path.GetFileName(filePath), System.StringComparison.Ordinal)));
            if (!Directory.Exists(fullPath))
            {
                System.Diagnostics.Debug.WriteLine($"PDFjsPageVM.SaveFileToDisk: Directory {fullPath} doesnt exist, creating");
                Directory.CreateDirectory(fullPath);
            }
            using (var memoryStream = new MemoryStream())
            {
                await pdfStream.CopyToAsync(memoryStream);
                File.WriteAllBytes(filePath, memoryStream.ToArray());
            }
            return filePath;
        }
    }
}
