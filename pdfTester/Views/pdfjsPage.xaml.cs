using System.Threading.Tasks;
using Xamarin.Forms;

namespace pdfTester
{
    public partial class pdfjsPage : ContentPage
    {
        public PDFjsPageVM VM
        {
            get { return BindingContext as PDFjsPageVM; }
        }

        public pdfjsPage(string url)
        {
            InitializeComponent();
            BindingContext = new PDFjsPageVM();

            Task.Run(() => VM.LoadUrl(url));
        }        
    }
}
