using System;
using System.IO;
using System.Threading.Tasks;
using Android;
using Android.App;
using pdfTester.Droid;
using pdfTester.Droid.Employment.Mobile.Base.AndroidUI;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalFileProvider))]
namespace pdfTester.Droid
{
    public class LocalFileProvider : ILocalFileProvider
    {
        public async Task RequestPermissionToSavePDF(Action<bool> resultAction)
        {
            if (await Android6PermissionsHelper.CheckPermissionAndRequestIfNotGranted
            (
                (Activity)Forms.Context,
                new[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage },
                PermissionRequestCode.EXTERNAL_STORAGE,
                "To be able to sync your appointments, please grant permission to access your calendar",
                (bool granted) =>
                {
                    System.Diagnostics.Debug.WriteLine($"LocalFileProvider.RequestPermissionToSavePDF: Permission {granted}, invoking callback");
                    resultAction?.Invoke(granted);
                }
            ) == false)
            {
                resultAction?.Invoke(false);
            }
        }
    }
}
