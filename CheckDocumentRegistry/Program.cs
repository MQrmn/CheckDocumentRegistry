
namespace CheckDocumentRegistry
{
    internal class Program
    {
        static void Main()
        {
            Arguments args = ParamsRepository.GetParams();

            //DocumentsLoader documents = new(args.doSpreadSheetPath, args.uppSpreadSheetPath);
            DocumentsLoader documents = new(args);

            Comparator compareResult = new(documents.doDocuments, documents.uppDocuments, documents.ignoreDoDocuments);

            ReportCreator.CreateReports(compareResult, args);

        }
    }
}