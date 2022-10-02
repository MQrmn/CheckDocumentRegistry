
namespace CheckDocumentRegistry
{
    internal class Program
    {
        static void Main()
        {

            // Getting program parameters
            ProgramParameters programParameters = ParametersReaderJSON.GetParameters();

            // Checkimg for existing files to comparingg
            WorkAbilityChecker.CheckFiles(programParameters);

            DocumentsLoader documentsLoader = new();

            // Getting documents from spreadsheets
            List<Document> doDocuments = documentsLoader.GetDoDocuments(programParameters.doSpreadSheetPath);
            List<Document> uppDocuments = documentsLoader.GetUppDocuments(programParameters.uppSpreadSheetPath);

            // Getting ignored documents from spreadsheets
            List<Document> ignoreDoDocuments = documentsLoader.GetIgnore(programParameters.doIgnoreSpreadSheetPath);
            List<Document> ignoreUppDocuments = documentsLoader.GetIgnore(programParameters.uppIgnoreSpreadSheetPath);

            // Comparing documents
            FullDocumentsComparator compareResult = new(doDocuments, uppDocuments, ignoreDoDocuments, ignoreUppDocuments);

            // Setting comments in unmatched documents about mismacthes
            PartialDocumentsComparator unmatchedDocumentsCommentator = new(compareResult.Documents1CDoUnmatched,
                                                                                compareResult.Documents1CUppUnmatched);
            unmatchedDocumentsCommentator.CommentUnmatchedDocuments();

            // Creating reports
            SpreadSheetWriterXLSX spreadSheetWriterPassedDo = new(programParameters.passedDoPath);
            spreadSheetWriterPassedDo.CreateSpreadsheet(compareResult.Documents1CDoUnmatched);

            SpreadSheetWriterXLSX spreadSheetWriterPassedUpp = new(programParameters.passedUppPath);
            spreadSheetWriterPassedUpp.CreateSpreadsheet(compareResult.Documents1CUppUnmatched);

            if (programParameters.printMatchedDocuments)
            {
                SpreadSheetWriterXLSX spreadSheetWriterMatchedDo = new(programParameters.matchedDoPath);
                spreadSheetWriterMatchedDo.CreateSpreadsheet(compareResult.Documents1CDoMatched);

                SpreadSheetWriterXLSX spreadSheetWriterMatcheUpp = new(programParameters.matchedUppPath);
                spreadSheetWriterMatcheUpp.CreateSpreadsheet(compareResult.Documents1CUppMatched);
            }

            if (programParameters.askAboutCloseProgram)
            {
                Console.WriteLine("Для завершения программы нажмите любую клавишу.");
                Console.ReadKey();
            }
        }
    }
}