
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
            FixedParameters fixedParameters;                    // Static parameters
            ChangeableParameters programParameters;             // Loaded from config file parameters
            DocumentsLoader documentsLoader;
            DocumentsAmount documentsAmount = new();

            GetParametersOrSetDefaults();                       // Getting program parameters
            WorkAbilityChecker.CheckFiles(programParameters);   // Checkimg for existing files to comparingg

            DocumentAmountReporter documentsAmountReporter = new(programParameters.reportFilePath);

            GetSourseDocoments();                               // Getting documents from spreadsheets
            GetIgnoreListsAndCounts();                          // Getting ignored documents from spreadsheets
            GetSourceDocumentsCounts();
            CompareDocuments();
            GetProcessedDocumentsCounts();
            GenerateOutputSpreadsheets();                       // Creating reports
            documentsAmountReporter.CreateAllReports(uppDocuments, documentsAmount);
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
                
                documentsAmount.doDocumentsCount = doDocuments.Count;
                documentsAmount.uppDocumentsCount = uppDocuments.Count;
                documentsAmount.ignoreDoDocumentsCount = ignoreDoDocuments.Count;
                documentsAmount.ignoreUppDocumentsCount = ignoreUppDocuments.Count;
            }

            void GetProcessedDocumentsCounts()
            {
                documentsAmount.Documents1CDoUnmatchedCount = compareResult.Documents1CDoUnmatched.Count;
                documentsAmount.Documents1CUppUnmatchedCount = compareResult.Documents1CUppUnmatched.Count;
                documentsAmount.Documents1CDoMatchedCount = compareResult.Documents1CDoMatched.Count;
                documentsAmount.Documents1CUppMatchedCount = compareResult.Documents1CUppMatched.Count;
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