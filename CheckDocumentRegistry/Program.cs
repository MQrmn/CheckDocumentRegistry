
namespace CheckDocumentRegistry
{
    internal class Program
    {
        static void Main()
        {
            ProgramParameters args = ParamsReaderJSON.GetParams();

            //DocumentsLoader documents = new(args.doSpreadSheetPath, args.uppSpreadSheetPath);
            DocumentsLoader documents = new(args);

            DocumentsComparator compareResult = new(documents.doDocuments, documents.uppDocuments, documents.ignoreDoDocuments);

            SpreadSheedCreator.CreateReports(compareResult, args);

        }
    }
}