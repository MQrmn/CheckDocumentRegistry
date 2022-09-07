
namespace DocumentsComparator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DocumentsComparator documentsComparator = new DocumentsComparator();

            DoReportGetter doReportGetter = new DoReportGetter();

            doReportGetter.GetDocument();
        }
    }
}