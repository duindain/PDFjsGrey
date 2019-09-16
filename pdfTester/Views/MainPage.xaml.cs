using System.ComponentModel;
using Xamarin.Forms;

namespace pdfTester
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPageVM VM
        {
            get { return BindingContext as MainPageVM; }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageVM();
        }

        void Handle_RandomPDFClick(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new pdfjsPage(VM.RandomPDF()));
        }

        void Handle_OpenPDFClick(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new pdfjsPage(VM.PDFUrl));
        }

        void Handle_PDFSelectedIndexChanged(object sender, System.EventArgs e)
        {
            var selectedIndex = (sender as Picker)?.SelectedIndex;
            if (selectedIndex >= 0)
            {
                var pdf = VM.PDFs[selectedIndex.Value];
                if (pdf != null)
                {
                    VM.PDFUrl = pdf.FilePath;
                    VM.LastOpenedPDF = pdf.FileName;
                    Handle_OpenPDFClick(null, null);
                }
            }                    
        }
    }
}
