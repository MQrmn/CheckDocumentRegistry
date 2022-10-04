
namespace CheckDocumentRegistry
{
    internal class Program
    {
        static void Main()
        {

            // Getting program parameters
            ProgramParameters programParameters = ParamsReadWriteJSON.GetParameters();

            // Checkimg for existing files to comparingg
            WorkAbilityChecker.CheckFiles(programParameters);

            // Getting documents from spreadsheets
            DocumentsLoader documentsLoader = new();

            
            List<Document> doDocuments = documentsLoader.GetDoDocuments(programParameters);
            List<Document> uppDocuments = documentsLoader.GetUppDocuments(programParameters);

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
            spreadSheetWriterPassedUpp.CreateSpreadsheet(compareResult.Documents1CUppUnmatched, false);

            if (programParameters.printMatchedDocuments)
            {
                SpreadSheetWriterXLSX spreadSheetWriterMatchedDo = new(programParameters.matchedDoPath);
                spreadSheetWriterMatchedDo.CreateSpreadsheet(compareResult.Documents1CDoMatched, false);

                SpreadSheetWriterXLSX spreadSheetWriterMatcheUpp = new(programParameters.matchedUppPath);
                spreadSheetWriterMatcheUpp.CreateSpreadsheet(compareResult.Documents1CUppMatched, false);
            }

            if (programParameters.askAboutCloseProgram)
            {
                Console.WriteLine("Для завершения программы нажмите любую клавишу.");
                Console.ReadKey();
            }
        }
    }
}