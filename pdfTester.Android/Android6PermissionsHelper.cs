using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;

namespace pdfTester.Droid
{
    namespace Employment.Mobile.Base.AndroidUI
    {
        public class Android6PermissionsHelper
        {
            //Map a request code to a resultAction
            static Dictionary<PermissionRequestCode, Action<bool>> resultActions = new Dictionary<PermissionRequestCode, Action<bool>>();

            public static async Task<bool> CheckPermissionAndRequestIfNotGranted(Activity activity, string[] permissions, PermissionRequestCode requestCode, string reason, Action<bool> resultAction)
            {
                if (activity == null || permissions.Length == 0 || requestCode == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Android6PermissionsHelper.RequestLocationPermissionsIfNotGranted: missing data");
                    return false;
                }

                //Only applicable to Android 6.0 runtime permission system
                if (Build.VERSION.SdkInt < BuildVersionCodes.M)
                {
                    //Do not need to grant permission 
                    System.Diagnostics.Debug.WriteLine("Android6PermissionsHelper.RequestLocationPermissionsIfNotGranted: do not need to grant permission");
                    resultAction?.Invoke(true);

                    return true;
                }

                //Check to see if all permission are granted
                if (permissions.All(a => ContextCompat.CheckSelfPermission(activity, a) == Permission.Granted))
                {
                    //Permission already granted
                    System.Diagnostics.Debug.WriteLine("Android6PermissionsHelper.RequestLocationPermissionsIfNotGranted: permission already granted");
                    resultAction?.Invoke(true);

                    return true;
                }

                //Need to request permission
                if (permissions.Any(a => ActivityCompat.ShouldShowRequestPermissionRationale(activity, a))
                    && string.IsNullOrEmpty(reason) == false)
                {
                    System.Diagnostics.Debug.WriteLine("Android6PermissionsHelper.RequestLocationPermissionsIfNotGranted: Ask for permission showing a reason.");

                    // Provide an additional rationale to the user if the permission was not granted
                    // and the user would benefit from additional context for the use of the permission.
                    // For example if the user has previously denied the permission.
                    //var messageService = ServiceFactory.Resolve<IMessageService>();
                    //if (await messageService.DisplayAlertMessageDialogForResult("Yeah!", reason, "OK", "Cancel"))
                    {
                        ActivityCompat.RequestPermissions(activity, permissions, (int)requestCode);
                    }
                }
                else
                {
                    //Not the first time or not IBaseView
                    System.Diagnostics.Debug.WriteLine("Android6PermissionsHelper.RequestLocationPermissionsIfNotGranted: Ask for permission without a reason.");
                    ActivityCompat.RequestPermissions(activity, permissions, (int)requestCode);
                }

                if (!resultActions.ContainsKey(requestCode))
                {
                    resultActions.Add(requestCode, resultAction);
                }

                //Have not got permission - wait for OnRequestPermissionsResult
                return false;
            }

            public static void ProcessRequestPermissionsResult(PermissionRequestCode requestCode, Permission[] grantResults)
            {
                // Received permission result
                System.Diagnostics.Debug.WriteLine($"ProcessRequestPermissionsResult.ProcessRequestPermissionsResult: Received response for {requestCode.ToString()} permission request.");

                var action = resultActions.ContainsKey(requestCode) ? resultActions[requestCode] : null;
                if (action == null)
                {
                    System.Diagnostics.Debug.WriteLine($"ProcessRequestPermissionsResult.ProcessRequestPermissionsResult: No action mapped to requestCode");
                    return;
                }

                // Check if the only required permission has been granted
                if (grantResults.All(a => a == Permission.Granted))
                {
                    action?.Invoke(true);

                    // Location permission has been granted, okay to retrieve the location of the device.
                    System.Diagnostics.Debug.WriteLine($"ProcessRequestPermissionsResult.ProcessRequestPermissionsResult: {requestCode.ToString()} permission has now been granted.");
                }
                else
                {
                    action?.Invoke(false);

                    //var messageService = ServiceFactory.Resolve<IMessageService>();
                    System.Diagnostics.Debug.WriteLine($"ProcessRequestPermissionsResult.ProcessRequestPermissionsResult: {requestCode.ToString()} permission was NOT granted.");
                    //messageService.DisplayAlertMessageDialog(DialogMessageTitles.Information, "Unable to fully use this feature without permission.");
                }
            }
        }
    }
}