using System;
using System.Threading.Tasks;

namespace pdfTester
{
    public interface ILocalFileProvider
    {
        Task RequestPermissionToSavePDF(Action<bool> resultAction);
    }
}
