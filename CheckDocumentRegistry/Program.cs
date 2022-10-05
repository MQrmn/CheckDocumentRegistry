
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
            FixedParameters fixedParameters;                    // Static parameters
            ChangeableParameters programParameters;             // Loaded from config file parameters
            DocumentsLoader documentsLoader;
            DocNumByCompaniesReporter reporterByCompany;

            GetParametersOrSetDefaults();                       // Getting program parameters
            WorkAbilityChecker.CheckFiles(programParameters);   // Checkimg for existing files to comparingg
            GetSourseDocoments();                               // Getting documents from spreadsheets
            GetIgnoreListsAndCounts();                          // Getting ignored documents from spreadsheets
            GetSourceDocumentsCounts();
            CompareDocuments();
            GetProcessedDocumentsCounts();
            GenerateOutputSpreadsheets();                       // Creating reports
            PrintCommonReportOnConsole();
            PrintReportByCompanies();
            CloseProgram();


            void GetParametersOrSetDefaults()
            {
                fixedParameters = new();
                programParameters = ParamsReadJSON.GetProgramParameters(fixedParameters.ProgramParametersFilePath);
                if (programParameters is null)
                {
                    programParameters.SetDefaults();
                    ParamsWriteJSON.WriteDefaultParams(programParameters, fixedParameters.ProgramParametersFilePath);
                    AdditionalInfoWriteTXT.WriteAdditionalInfo();
                }
            }

            void GetSourseDocoments()
            {
                documentsLoader = new();
                doDocuments = documentsLoader.GetDoDocuments(programParameters.doSpreadSheetPath, 
                                                               programParameters.exceptedDoPath);
                uppDocuments = documentsLoader.GetUppDocuments(programParameters.uppSpreadSheetPath,
                                                               programParameters.exceptedUppPath);
            }

            void GetIgnoreListsAndCounts()
            {
                ignoreDoDocuments = documentsLoader.GetIgnore(programParameters.doIgnoreSpreadSheetPath);
                ignoreUppDocuments = documentsLoader.GetIgnore(programParameters.uppIgnoreSpreadSheetPath);
                
            }

            void CompareDocuments()
            {
                compareResult = new(doDocuments, uppDocuments, ignoreDoDocuments, ignoreUppDocuments);
                
                // Setting comments in unmatched documents about mismacthes
                unmatchedDocumentsCommentator = new(compareResult.Documents1CDoUnmatched,
                                                                                    compareResult.Documents1CUppUnmatched);
                unmatchedDocumentsCommentator.CommentUnmatchedDocuments();
            }



            void GenerateOutputSpreadsheets()
            {
                spreadSheetWriterPassedDo = new(programParameters.passedDoPath);
                spreadSheetWriterPassedDo.CreateSpreadsheet(compareResult.Documents1CDoUnmatched);
                spreadSheetWriterPassedUpp = new(programParameters.passedUppPath);
                spreadSheetWriterPassedUpp.CreateSpreadsheet(compareResult.Documents1CUppUnmatched, false);

                if (programParameters.printMatchedDocuments)
                {
                    spreadSheetWriterMatchedDo = new(programParameters.matchedDoPath);
                    spreadSheetWriterMatchedDo.CreateSpreadsheet(compareResult.Documents1CDoMatched, false);

                    spreadSheetWriterMatcheUpp = new(programParameters.matchedUppPath);
                    spreadSheetWriterMatcheUpp.CreateSpreadsheet(compareResult.Documents1CUppMatched, false);
                }
            }

            void GetSourceDocumentsCounts()
            {
                doDocumentsCount = doDocuments.Count;
                uppDocumentsCount = uppDocuments.Count;
                ignoreDoDocumentsCount = ignoreDoDocuments.Count;
                ignoreUppDocumentsCount = ignoreUppDocuments.Count;
            }

            void GetProcessedDocumentsCounts()
            {
                Documents1CDoUnmatchedCount = compareResult.Documents1CDoUnmatched.Count;
                Documents1CUppUnmatchedCount = compareResult.Documents1CUppUnmatched.Count;
                Documents1CDoMatchedCount = compareResult.Documents1CDoMatched.Count;
                Documents1CUppMatchedCount = compareResult.Documents1CUppMatched.Count;
            }

            void PrintReportByCompanies()
            {
                reporterByCompany = new();
                reporterByCompany.PrintReport(uppDocuments);
            }

            void PrintCommonReportOnConsole()
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nРезультат сравнения документов, внесеннных в 1С:ДО, с реестром 1С:УПП");
                Console.WriteLine("От " + DateTime.Now.ToLongDateString() + ":\n");
                Console.WriteLine("Документов 1С:ДО всего: " + doDocumentsCount);
                Console.WriteLine("Документов 1С:УПП всего: " + uppDocumentsCount);
                Console.WriteLine("Документов 1С:ДО в игнор-листе: " + ignoreDoDocumentsCount);
                Console.WriteLine("Документов 1С:УПП в игнор-листе: " + ignoreUppDocumentsCount);
                Console.WriteLine("Документов 1С:ДО совпавпало:  " + Documents1CDoMatchedCount);
                Console.WriteLine("Документов 1С:УПП совпавпало: " + Documents1CUppMatchedCount);
                Console.WriteLine("Документов 1С:ДО не совпало c 1С:УПП: " + Documents1CDoUnmatchedCount);
                Console.WriteLine("Документов 1С:УПП не совпало 1С:ДО: " + Documents1CUppUnmatchedCount);
                Console.ResetColor();
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