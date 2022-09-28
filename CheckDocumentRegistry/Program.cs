
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
            FullDocumentsComparator compareResult = new(doDocuments,
                                                   uppDocuments, 
                                                   ignoreDoDocuments,
                                                   ignoreUppDocuments);

            PartialDocumentsComparator unmatchedDocumentsCommentator = new(compareResult.Documents1CDoUnmatched,
                                                                                compareResult.Documents1CUppUnmatched);

            unmatchedDocumentsCommentator.CommentUnmatchedDocuments();


            // Creating reports
            SpreadSheetWriterXLSX.Create(compareResult.Documents1CDoMatched, programParameters.matchedDoPath);
            SpreadSheetWriterXLSX.Create(compareResult.Documents1CUppMatched, programParameters.matchedUppPath);
            SpreadSheetWriterXLSX.Create(compareResult.Documents1CDoUnmatched, programParameters.passedDoPath);
            SpreadSheetWriterXLSX.Create(compareResult.Documents1CUppUnmatched, programParameters.passedUppPath);
            

        }
    }
}