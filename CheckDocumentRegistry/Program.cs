
namespace CheckDocumentRegistry
{
    internal class Program
    {
        static void Main()
        {
            
            List<Document> doDocuments;                         // Source 1C:DO documents for compare
            List<Document> uppDocuments;                        // Source 1C:UPP documents for compare
            List<Document> ignoreDoDocuments;                   // Ignore list of 1C:DO documents 
            List<Document> ignoreUppDocuments;                  // Ignore list of 1C:UPP documents 
            FullDocumentsComparator compareResult;              // Class contains results of documents comparing 
            PartialDocumentsComparator unmatchedDocumentsCommentator;  // Class set comments in unmatched documents
            SpreadSheetWriterXLSX spreadSheetWriterPassedDo;    // Spreadsheet creator
            SpreadSheetWriterXLSX spreadSheetWriterPassedUpp;   // Spreadsheet creator
            SpreadSheetWriterXLSX spreadSheetWriterMatchedDo;   // Spreadsheet creator
            SpreadSheetWriterXLSX spreadSheetWriterMatcheUpp;   // Spreadsheet creator
            InternalParameters fixedParameters;                    // Static parameters
            UserParameters programParameters;             // Loaded from config file parameters
            DocumentsLoader documentsLoader;
            DocumentsAmount documentsAmount = new();

            GetProgramParameters();                             // Getting program parameters
            WorkAbilityChecker.CheckFiles(programParameters);   // Checkimg for existing files to comparingg
            GetSourseDocoments();                               // Getting documents from spreadsheets
            GetIgnoreListsAndCounts();                          // Getting ignored documents from spreadsheets
            GetSourceDocumentsCounts();
            CompareDocuments();
            GetDocumentsAmounts();

            // Creating reports
            DocumentAmountReporter documentsAmountReporter = new(programParameters.reportFilePath);
            documentsAmountReporter.CreateAllReports(uppDocuments, documentsAmount);
            GenerateOutputSpreadsheets();                       
            CloseProgram();


            void GetProgramParameters()
            {
                fixedParameters = new();
                ProgramParametersReadWrite programParameters1 = new(fixedParameters.ProgramParametersFilePath);
                programParameters = programParameters1.GetProgramParameters();
            }


            void GetSourseDocoments()
            {
                documentsLoader = new();
                doDocuments = documentsLoader.GetDocuments1CDO(programParameters.doSpreadSheetPath, 
                                                               programParameters.exceptedDoPath);
                uppDocuments = documentsLoader.GetDocuments1CUPP(programParameters.uppSpreadSheetPath,
                                                               programParameters.exceptedUppPath);
            }


            void GetIgnoreListsAndCounts()
            {
                ignoreDoDocuments = documentsLoader.GetIgnoreDocs(programParameters.doIgnoreSpreadSheetPath);
                ignoreUppDocuments = documentsLoader.GetIgnoreDocs(programParameters.uppIgnoreSpreadSheetPath);
                
            }


            void CompareDocuments()
            {
                compareResult = new(doDocuments, uppDocuments, ignoreDoDocuments, ignoreUppDocuments);
                unmatchedDocumentsCommentator = new(compareResult.UnmatchedDocuments1CDO,
                                                                                    compareResult.UnmatchedDocuments1CUPP);
                unmatchedDocumentsCommentator.CommentUnmatchedDocuments();
            }


            void GenerateOutputSpreadsheets()
            {
                spreadSheetWriterPassedDo = new(programParameters.passedDoPath);
                spreadSheetWriterPassedDo.CreateSpreadsheet(compareResult.UnmatchedDocuments1CDO);
                spreadSheetWriterPassedUpp = new(programParameters.passedUppPath);
                spreadSheetWriterPassedUpp.CreateSpreadsheet(compareResult.UnmatchedDocuments1CUPP, false);

                if (programParameters.printMatchedDocuments)
                {
                    spreadSheetWriterMatchedDo = new(programParameters.matchedDoPath);
                    spreadSheetWriterMatchedDo.CreateSpreadsheet(compareResult.MatchedDocuments1CDO, false);
                    spreadSheetWriterMatcheUpp = new(programParameters.matchedUppPath);
                    spreadSheetWriterMatcheUpp.CreateSpreadsheet(compareResult.MatchedDocuments1CUPP, false);
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
                documentsAmount.Documents1CDoUnmatchedCount = compareResult.UnmatchedDocuments1CDO.Count;
                documentsAmount.Documents1CUppUnmatchedCount = compareResult.UnmatchedDocuments1CUPP.Count;
                documentsAmount.Documents1CDoMatchedCount = compareResult.MatchedDocuments1CDO.Count;
                documentsAmount.Documents1CUppMatchedCount = compareResult.MatchedDocuments1CUPP.Count;
            }


            void CloseProgram()
            {
                if (programParameters.askAboutCloseProgram)
                {
                    Console.WriteLine("Для завершения программы нажмите любую клавишу.");
                    Console.ReadKey();
                }
            }
        }
    }
}