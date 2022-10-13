
namespace RegComparator
{
    internal class Program
    {
        static void Main()
        {
            
            List<Document> doDocuments;                         // Source 1C:DO documents for compare
            List<Document> uppDocuments;                        // Source 1C:UPP documents for compare
            List<Document> ignoreDoDocuments;                   // Ignore list of 1C:DO documents 
            List<Document> ignoreUppDocuments;                  // Ignore list of 1C:UPP documents 
            FullDocComparator compareResult;              // Class contains results of documents comparing 
            UnmatchedDocCommentSetter unmatchedDocumentsCommentator;  // Class set comments in unmatched documents
            SpreadSheetWriterXLSX spreadSheetWriterPassedDo;    // Spreadsheet creator
            SpreadSheetWriterXLSX spreadSheetWriterPassedUpp;   // Spreadsheet creator
            SpreadSheetWriterXLSX spreadSheetWriterMatchedDo;   // Spreadsheet creator
            SpreadSheetWriterXLSX spreadSheetWriterMatcheUpp;   // Spreadsheet creator
            InternalParameters internalParameters;                    // Static parameters
            UserParameters userParameters;             // Loaded from config file parameters
            DocLoader documentsLoader;
            DocumentsAmount documentsAmount = new();

            GetProgramParameters();                             // Getting program parameters
            WorkAbilityChecker.CheckFiles(userParameters);   // Checkimg for existing files to comparingg
            GetSourseDocoments();                               // Getting documents from spreadsheets
            GetIgnoreListsAndCounts();                          // Getting ignored documents from spreadsheets
            GetSourceDocumentsCounts();
            CompareDocuments();
            GetDocumentsAmounts();

            // Creating reports
            DocumentAmountReporter documentsAmountReporter = new(userParameters.reportFilePath);
            documentsAmountReporter.CreateAllReports(uppDocuments, documentsAmount);
            GenerateOutputSpreadsheets();                       
            CloseProgram();


            void GetProgramParameters()
            {
                internalParameters = new();
                ProgramParametersReadWrite programParameters1 = new(internalParameters.ProgramParametersFilePath);
                userParameters = programParameters1.GetProgramParameters();
            }


            void GetSourseDocoments()
            {
                documentsLoader = new();
                doDocuments = documentsLoader.GetDocs1CDO(userParameters.doSpreadSheetPath, 
                                                               userParameters.exceptedDoPath);
                uppDocuments = documentsLoader.GetDocs1CUPP(userParameters.uppSpreadSheetPath,
                                                               userParameters.exceptedUppPath);
            }


            void GetIgnoreListsAndCounts()
            {
                ignoreDoDocuments = documentsLoader.GetDocsPass(userParameters.doIgnoreSpreadSheetPath);
                ignoreUppDocuments = documentsLoader.GetDocsPass(userParameters.uppIgnoreSpreadSheetPath);
                
            }


            void CompareDocuments()
            {
                compareResult = new(doDocuments, uppDocuments, ignoreDoDocuments, ignoreUppDocuments);
                unmatchedDocumentsCommentator = new(compareResult.UnmatchedDocs1CDO,
                                                                                    compareResult.UnmatchedDocs1CUPP);
                unmatchedDocumentsCommentator.CommentUnmatchedDocuments();
            }


            void GenerateOutputSpreadsheets()
            {
                spreadSheetWriterPassedDo = new(userParameters.passedDoPath);
                spreadSheetWriterPassedDo.CreateSpreadsheet(compareResult.UnmatchedDocs1CDO);
                spreadSheetWriterPassedUpp = new(userParameters.passedUppPath);
                spreadSheetWriterPassedUpp.CreateSpreadsheet(compareResult.UnmatchedDocs1CUPP, false);

                if (userParameters.printMatchedDocuments)
                {
                    spreadSheetWriterMatchedDo = new(userParameters.matchedDoPath);
                    spreadSheetWriterMatchedDo.CreateSpreadsheet(compareResult.MatchedDocs1CDO, false);
                    spreadSheetWriterMatcheUpp = new(userParameters.matchedUppPath);
                    spreadSheetWriterMatcheUpp.CreateSpreadsheet(compareResult.MatchedDocs1CUPP, false);
                }
            }


            void GetSourceDocumentsCounts()
            {
                documentsAmount.doDocumentsCount = doDocuments.Count;
                documentsAmount.uppDocumentsCount = uppDocuments.Count;
                documentsAmount.ignoreDoDocumentsCount = ignoreDoDocuments.Count;
                documentsAmount.ignoreUppDocumentsCount = ignoreUppDocuments.Count;
            }


            void GetDocumentsAmounts()
            {
                documentsAmount.Documents1CDoUnmatchedCount = compareResult.UnmatchedDocs1CDO.Count;
                documentsAmount.Documents1CUppUnmatchedCount = compareResult.UnmatchedDocs1CUPP.Count;
                documentsAmount.Documents1CDoMatchedCount = compareResult.MatchedDocs1CDO.Count;
                documentsAmount.Documents1CUppMatchedCount = compareResult.MatchedDocs1CUPP.Count;
            }


            void CloseProgram()
            {
                if (userParameters.askAboutCloseProgram)
                {
                    Console.WriteLine("Для завершения программы нажмите любую клавишу.");
                    Console.ReadKey();
                }
            }
        }
    }
}