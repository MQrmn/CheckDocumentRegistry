
namespace CheckDocumentRegistry
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Arguments arguments = new Arguments(args);

            //ArgsParser parsedArgs = new ArgsParser(args);

            DocumentsLoader documents = new DocumentsLoader(arguments.doSpreadSheetPath, arguments.uppSpreadSheetPath);

            Comparator comparator = new Comparator(documents.doDocuments, documents.uppDocuments);

            ReportCreator.CreateReports(comparator, arguments);

        }
    }
}