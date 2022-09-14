
namespace CheckDocumentRegistry
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ArgsParser arguments = new(args);

            DocumentsLoader documents = new(arguments.doSpreadSheetPath, arguments.uppSpreadSheetPath);

            Comparator compareResult = new(documents.doDocuments, documents.uppDocuments);

            ReportCreator.CreateReports(compareResult, arguments);

        }
    }
}