using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace pdfTester
{
    public class MainPageVM : BaseViewModel
    {
        private string _pdfUrl;
        public string PDFUrl
        {
            get => _pdfUrl;
            set => SetProperty(ref _pdfUrl, value);
        }

        private ObservableCollection<PDFSelectItem> _pdfs;
        public ObservableCollection<PDFSelectItem> PDFs
        {
            get => _pdfs;
            set => SetProperty(ref _pdfs, value);
        }

        private string _lastOpenedPDF;
        public string LastOpenedPDF
        {
            get => _lastOpenedPDF;
            set => SetProperty(ref _lastOpenedPDF, value);
        }

        public MainPageVM()
        {
            PDFs = new ObservableCollection<PDFSelectItem>(new List<PDFSelectItem>
            {
                new PDFSelectItem {FilePath = "https://media.wuerth.com/stmedia/shop/catalogpages/LANG_it/1637048.pdf", FileName = "wuerth example pdf" },
                new PDFSelectItem {FilePath = "https://www.ets.org/Media/Tests/GRE/pdf/gre_research_validity_data.pdf", FileName = "gre_research_validity_data" },
                new PDFSelectItem {FilePath = "https://file-examples.com/wp-content/uploads/2017/10/file-sample_150kB.pdf", FileName = "file-sample_150kB" },
                new PDFSelectItem {FilePath = "https://file-examples.com/wp-content/uploads/2017/10/file-example_PDF_500_kB.pdf", FileName = "file-example_PDF_500_kB" },
                new PDFSelectItem {FilePath = "https://www.adobe.com/support/products/enterprise/knowledgecenter/media/c4611_sample_explain.pdf", FileName = "c4611_sample_explain" },
                new PDFSelectItem {FilePath = "https://www.orimi.com/pdf-test.pdf", FileName = "orimi pdf-test" },
                new PDFSelectItem {FilePath = "https://gahp.net/wp-content/uploads/2017/09/sample.pdf", FileName="gahp example pdf"},
                new PDFSelectItem {FilePath = "https://xamarin.azureedge.net/developer/xamarin-forms-book/BookPreview2-Ch18-Rel0417.pdf", FileName="BookPreview2-Ch18-Rel0417"},
            });
        }

        public string RandomPDF()
        {
            var pdfFilePath = string.Empty;
            var randomPDF = PDFs.ElementAtOrDefault(Utilities.Random.Next(0, PDFs.Count));
            if(randomPDF != null)
            {
                LastOpenedPDF = randomPDF.FileName;
                pdfFilePath = randomPDF.FilePath;
            }
            return pdfFilePath;
        }
    }
}
