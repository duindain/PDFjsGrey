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

        private string _localFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "pdfs");

        /// <summary>
        /// Saves a pdf from a remote url to local file system then triggers opening it in pdfjs on Android or native webview in iOS
        /// Requests permissions on Android using dependancy service for ILocalFileProvider
        /// If the file already exists on the local file system it is not redownloaded
        /// </summary>
        /// <param name="url">pdf url to fetch and or open</param>
        /// <returns></returns>
        public async Task LoadUrl(string url)
        {
            var localPath = string.Empty;

            //We need custom behaviour on Android to request permission to access the filesystem to save the PDF locally
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
                        httpClient.Timeout = new TimeSpan(0,0,10);
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
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"PDFjsPageVM.ctor: Http error, network issue? {ex.Message}");
                    }                    
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"PDFjsPageVM.ctor: File exists locally {localPath}");
                OpenPdf(localPath, url);
            }
        }

        /// <summary>
        /// Sets the PDFSource variable to update the UI using pdfjs on Android with localPath or a native webview in iOS using url
        /// </summary>
        /// <param name="localPath">android file to open with pdfjs</param>
        /// <param name="url">iOS url to open using native webview</param>
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

        /// <summary>
        /// Creates a subdirectory in the Environment.SpecialFolder.MyDocuments directory called pdfs if it doesnt already exist
        /// Then writes out the PDF file as bytes using .net standard c# methods no platform specifics needed
        /// </summary>
        /// <param name="pdfStream">data to save to filesystem</param>
        /// <param name="filePath">path including filename to save to</param>
        /// <returns></returns>
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
