using Android.Content;
using Android.Views;
using Android.Webkit;
using pdfTester.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.WebView), typeof(PdfWebViewRenderer))]
namespace pdfTester.Droid.Renderers
{
    public class PdfWebViewRenderer : WebViewRenderer
    {
        public PdfWebViewRenderer(Context context) : base(context)
        {
            //WebView.SetWebContentsDebuggingEnabled(true);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.Settings.JavaScriptEnabled = true;
                Control.Settings.DomStorageEnabled = true;
                Control.Settings.AllowFileAccess = true;
                Control.Settings.AllowFileAccessFromFileURLs = true;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                               
                Control.SetWebChromeClient(new WebChromeClient());
            }
        }

        // If you want to enable scrolling in WebView uncomment the following lines.
        public override bool DispatchTouchEvent(MotionEvent e)
        {
            Parent.RequestDisallowInterceptTouchEvent(true);
            return base.DispatchTouchEvent(e);
        }
    }
}
