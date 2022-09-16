
namespace CheckDocumentRegistry
{
    internal class Program
    {
        static void Main()
        {
            Arguments args = ParamsRepository.GetParams();

            DocumentsLoader documents = new(args.doSpreadSheetPath, args.uppSpreadSheetPath);

            Comparator compareResult = new(documents.doDocuments, documents.uppDocuments);

            ReportCreator.CreateReports(compareResult, args);

        }
    }
}