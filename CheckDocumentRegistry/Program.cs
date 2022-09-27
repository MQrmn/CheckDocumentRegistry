
namespace CheckDocumentRegistry
{
    internal class Program
    {
        static void Main()
        {
            // Getting program parameters
            ProgramParameters programParameters = ParametersReaderJSON.GetParameters();

            // Checkimg for existing files to comparingg
            WorkAbilityChecker.CheckSourceFiles(programParameters);

            DocumentsLoader documentsLoader = new();

            // Getting documents from spreadsheets
            List<Document> doDocuments = documentsLoader.GetDoDocuments(programParameters.doSpreadSheetPath);
            List<Document> uppDocuments = documentsLoader.GetUppDocuments(programParameters.uppSpreadSheetPath);
            List<Document> ignoreDoDocuments = documentsLoader.GetIgnore(programParameters.doIgnoreSpreadSheetPath);
            List<Document> ignoreUppDocuments = documentsLoader.GetIgnore(programParameters.uppIgnoreSpreadSheetPath);

            // Comparing documents
            DocumentsComparator compareResult = new(doDocuments,
                                                   uppDocuments, 
                                                   ignoreDoDocuments,
                                                   ignoreUppDocuments);

            // Creating reports
            SpreadSheetWriterXLSX.Create(compareResult.matchedUppDocuments, programParameters.matchedUppPath);
            SpreadSheetWriterXLSX.Create(compareResult.matchedDoDocuments, programParameters.matchedDoPath);
            SpreadSheetWriterXLSX.Create(compareResult.uppDocuments, programParameters.passedUppPath);
            SpreadSheetWriterXLSX.Create(compareResult.doDocuments, programParameters.passedDoPath);

        }
    }
}