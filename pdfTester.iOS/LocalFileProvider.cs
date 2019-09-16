using System;
using System.IO;
using System.Threading.Tasks;
using pdfTester.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalFileProvider))]
namespace pdfTester.iOS
{
    public class LocalFileProvider : ILocalFileProvider
    {
        public async Task RequestPermissionToSavePDF(Action<bool> resultAction)
        {
            resultAction?.Invoke(true);
        }
    }
}
