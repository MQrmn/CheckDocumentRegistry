
namespace RegComparator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            List<Document> documentsDocManage;                  // Source 1C:DO documents for compare
            List<Document> documentsRegistry;                   // Source 1C:UPP documents for compare
            List<Document> passDocumentsDocManage;              // Ignore list of 1C:DO documents 
            List<Document> passDocumentsRegistry;               // Ignore list of 1C:UPP documents
            FullDocComparator compareResult;                    // Class contains results of documents comparing 
            UnmatchedDocCommentSetter unmatchedDocumentsCommentator;  // Class set comments in unmatched documents
            SpreadSheetWriterXLSX spreadsheetWriterPassedDo;    // Spreadsheet creator
            SpreadSheetWriterXLSX spreadSheetWriterPassedUpp;   // Spreadsheet creator
            SpreadSheetWriterXLSX spreadSheetWriterMatchedDo;   // Spreadsheet creator
            SpreadSheetWriterXLSX spreadSheetWriterMatcheUpp;   // Spreadsheet creator
            InternalParameters internalParameters;              // Static parameters
            UserParameters userParameters;
            DocLoader documentsLoader;
            DocumentsAmount documentsAmount = new();

            GetProgramParameters(args);                         // Getting program parameters
            WorkAbilityChecker.CheckFiles(userParameters);    // Checkimg for existing files to comparing

            GetSourseDocuments1CDO();

            if (userParameters.programMode == "UPP")
                GetSourseDocuments1CUPP();                      // Getting documents from spreadsheets
            else
                GetSourseDocuments1CKA();

            GetIgnoreLists();                                   // Getting ignored documents from spreadsheets
            GetSourceDocumentsCounts();
            CompareDocuments();
            GetDocumentsAmounts();
            DocumentAmountReporter documentsAmountReporter = new(userParameters.programReportFilePath);
            documentsAmountReporter.CreateAllReports(documentsRegistry, documentsAmount);
            GenerateOutputSpreadsheets();
            CloseProgram();


            void ArgsHandler(string[] args)
            { 
                if(args.Length < 2)
                    return;
                if (args[0] == "-c")
                {
                    internalParameters = new();
                    internalParameters.ProgramParametersFilePath = args[1];
                }
            }

            void GetProgramParameters(string[] args)
            {
                internalParameters = new();
                ArgsHandler(args);
                ProgramParametersReadWrite programParameters1 = new(internalParameters.ProgramParametersFilePath);
                userParameters = programParameters1.GetProgramParameters();
            }

            void GetSourseDocuments1CDO()
            {
                documentsLoader = new();
                documentsDocManage = documentsLoader.GetDocs1CDO(userParameters.inputSpreadsheetDocManagePath,
                                                               userParameters.exceptedDocManagePath);
            }


            void GetSourseDocuments1CUPP()
            {
                documentsLoader = new();
                documentsRegistry = documentsLoader.GetDocs1CUPP(userParameters.inputSpreadsheetDocRegistryPath[0],
                                                               userParameters.exceptedDocRegistryPath);
            }

            void GetSourseDocuments1CKA()
            {
                List<Document> documentsRegistryKaPart1;
                List<Document> documentsRegistryKaPart2;
                documentsLoader = new();
                documentsRegistryKaPart1 = documentsLoader.GetDocs1CKASf(userParameters.inputSpreadsheetDocRegistryPath[0],
                                                               userParameters.exceptedDocManagePath);

                documentsRegistryKaPart2 = documentsLoader.GetDocs1CKATn(userParameters.inputSpreadsheetDocRegistryPath[1],
                                                               userParameters.exceptedDocManagePath);

                documentsRegistry = documentsRegistryKaPart1.Concat(documentsRegistryKaPart2).ToList();

            }


            void GetIgnoreLists()
            {
                passDocumentsDocManage = documentsLoader.GetDocsPass(userParameters.passSpreadsheetDocManagePath);
                passDocumentsRegistry = documentsLoader.GetDocsPass(userParameters.passSpreadSheetDocRegistryPath);
            }


            void CompareDocuments()
            {
                compareResult = new(documentsDocManage, documentsRegistry, passDocumentsDocManage, passDocumentsRegistry);
                unmatchedDocumentsCommentator = new(compareResult.UnmatchedDocs1CDO,
                                                                                    compareResult.UnmatchedDocs1CUPP);
                unmatchedDocumentsCommentator.CommentUnmatchedDocuments();
            }


            void GenerateOutputSpreadsheets()
            {
                spreadsheetWriterPassedDo = new(userParameters.outputUnmatchDocManagePath);
                spreadsheetWriterPassedDo.CreateSpreadsheet(compareResult.UnmatchedDocs1CDO);
                spreadSheetWriterPassedUpp = new(userParameters.outputUnmatchedDocRegistryPath);
                spreadSheetWriterPassedUpp.CreateSpreadsheet(compareResult.UnmatchedDocs1CUPP, false);

                if (userParameters.isPrintMatchedDocuments)
                {
                    spreadSheetWriterMatchedDo = new(userParameters.outputMatchedDocManagePath);
                    spreadSheetWriterMatchedDo.CreateSpreadsheet(compareResult.MatchedDocs1CDO, false);
                    spreadSheetWriterMatcheUpp = new(userParameters.outputMatchedDocRestryPath);
                    spreadSheetWriterMatcheUpp.CreateSpreadsheet(compareResult.MatchedDocs1CUPP, false);
                }
            }


            void GetSourceDocumentsCounts()
            {
                documentsAmount.doDocumentsCount = documentsDocManage.Count;
                documentsAmount.uppDocumentsCount = documentsRegistry.Count;
                documentsAmount.ignoreDoDocumentsCount = passDocumentsDocManage.Count;
                documentsAmount.ignoreUppDocumentsCount = passDocumentsRegistry.Count;
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
                if (userParameters.isAskAboutCloseProgram)
                {
                    Console.WriteLine("Для завершения программы нажмите любую клавишу.");
                    Console.ReadKey();
                }
            }
        }
    }
}