
namespace CheckDocumentRegistry
{
    internal class Program
    {
        static void Main()
        {

            int doDocumentsCount;
            int uppDocumentsCount;

            int ignoreDoDocumentsCount;
            int ignoreUppDocumentsCount;

            int Documents1CDoUnmatchedCount;
            int Documents1CUppUnmatchedCount;

            int Documents1CDoMatchedCount;
            int Documents1CUppMatchedCount;




            // Getting program parameters
            ProgramParameters programParameters = ParamsReadWriteJSON.GetParameters();

            // Checkimg for existing files to comparingg
            WorkAbilityChecker.CheckFiles(programParameters);

            // Getting documents from spreadsheets
            DocumentsLoader documentsLoader = new();

            
            List<Document> doDocuments = documentsLoader.GetDoDocuments(programParameters);
            doDocumentsCount = doDocuments.Count;

            List<Document> uppDocuments = documentsLoader.GetUppDocuments(programParameters);
            uppDocumentsCount = uppDocuments.Count;

            // Getting ignored documents from spreadsheets
            List<Document> ignoreDoDocuments = documentsLoader.GetIgnore(programParameters.doIgnoreSpreadSheetPath);
            ignoreDoDocumentsCount = ignoreDoDocuments.Count;

            List<Document> ignoreUppDocuments = documentsLoader.GetIgnore(programParameters.uppIgnoreSpreadSheetPath);
            ignoreUppDocumentsCount = ignoreUppDocuments.Count;

            // Comparing documents
            FullDocumentsComparator compareResult = new(doDocuments, uppDocuments, ignoreDoDocuments, ignoreUppDocuments);

            // Setting comments in unmatched documents about mismacthes
            PartialDocumentsComparator unmatchedDocumentsCommentator = new(compareResult.Documents1CDoUnmatched,
                                                                                compareResult.Documents1CUppUnmatched);
            unmatchedDocumentsCommentator.CommentUnmatchedDocuments();

            Documents1CDoUnmatchedCount = compareResult.Documents1CDoUnmatched.Count;
            Documents1CUppUnmatchedCount = compareResult.Documents1CUppUnmatched.Count;

            Documents1CDoMatchedCount = compareResult.Documents1CDoMatched.Count;
            Documents1CUppMatchedCount = compareResult.Documents1CUppMatched.Count;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + "Документов 1С:ДО загружено: " + doDocumentsCount);
            Console.WriteLine("Документов 1С:УПП загружено: " + uppDocumentsCount);
            Console.WriteLine("Документов 1С:ДО в игнор-листе: " + ignoreDoDocumentsCount);
            Console.WriteLine("Документов 1С:УПП в игнор-листе: " + ignoreUppDocumentsCount);
            Console.WriteLine("Документов 1С:ДО не совпавших: " + Documents1CDoUnmatchedCount);
            Console.WriteLine("Документов 1С:УПП не совпавших: " + Documents1CUppUnmatchedCount);
            Console.WriteLine("Документов 1С:ДО совпавших:  " + Documents1CDoMatchedCount);
            Console.WriteLine("Документов 1С:УПП совпавших: " + Documents1CUppMatchedCount + "\n");
            Console.ResetColor();


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