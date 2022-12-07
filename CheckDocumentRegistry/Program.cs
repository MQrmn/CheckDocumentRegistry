
namespace RegComparator
{
    internal class Program
    {
        static void Main(string[] args)
        { 
            List<Document> docs1CDO;                                        // Source 1C:DO documents for compare
            List<Document> docsRegistry;                                    // Source 1C:UPP, 1C:KA documents for compare
            List<Document> passDocs1CDO;                                    // Ignore list of 1C:DO documents 
            List<Document> passDocsRegistry;                                // Ignore list of 1C:UPP documents
            DocComparator docComparator;                                    // Class contains results of documents comparing 
            UnmatchedDocCommentSetter unmatchedDocsCommentator;             // Class set comments in unmatched documents
            ConfFilesPath internalParameters;                               // Static parameters
            DocLoader docLoader;

            ReportDocAmount reportDocAmount = new();
            WorkParams workParams = GetWorkParams(args);                    // Getting program parameters
            WorkAbilityChecker.CheckFiles(workParams);                      // Checkimg for existing files to comparing

            GetSrcDocs1CDO();

            if (workParams.programMode == "UPP")
                GetSrcDocs1CUPP();                                          // Getting documents from spreadsheets
            else
                GetSrcDocs1CKA();

            GetIgnoreDocList();                                             // Getting ignored documents from spreadsheets
            GetSrcDocsAmount();
            CompareDocuments();
            GetResultDocsAmount();

            DocumentAmountReporter documentsAmountReporter = new(workParams.programReportFilePath);
            documentsAmountReporter.CreateAllReports(docsRegistry, reportDocAmount);

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


            WorkParams GetWorkParams(string[] args)
            {
                internalParameters = new();
                ArgsHandler(args);
                ProgramParametersReadWrite programParameters1 = new(internalParameters.ProgramParametersFilePath);
                return programParameters1.GetProgramParameters();
            }


            void GetSrcDocs1CDO()
            {
                docLoader = new();
                docs1CDO = docLoader.GetDocs1CDO(workParams.inputSpreadsheetDocManagePath,
                                                               workParams.exceptedDocManagePath);
            }


            void GetSrcDocs1CUPP()
            {
                docLoader = new();
                docsRegistry = docLoader.GetDocs1CUPP(workParams.inputSpreadsheetDocRegistryPath[0],
                                                               workParams.exceptedDocRegistryPath);
            }

            void GetSrcDocs1CKA()
            {
                List<Document> documentsRegistryKaPart1;
                List<Document> documentsRegistryKaPart2;
                docLoader = new();
                documentsRegistryKaPart1 = docLoader.GetDocs1CKASf(workParams.inputSpreadsheetDocRegistryPath[0],
                                                               workParams.exceptedDocManagePath);

                documentsRegistryKaPart2 = docLoader.GetDocs1CKATn(workParams.inputSpreadsheetDocRegistryPath[1],
                                                               workParams.exceptedDocManagePath);

                docsRegistry = documentsRegistryKaPart1.Concat(documentsRegistryKaPart2).ToList();
            }


            void GetIgnoreDocList()
            {
                passDocs1CDO = docLoader.GetDocsPass(workParams.passSpreadsheetDocManagePath);
                passDocsRegistry = docLoader.GetDocsPass(workParams.passSpreadSheetDocRegistryPath);
            }


            void CompareDocuments()
            {
                docComparator = new(docs1CDO, docsRegistry, passDocs1CDO, passDocsRegistry);
                unmatchedDocsCommentator = new(docComparator.UnmatchedDocs1CDO,
                                                                                    docComparator.UnmatchedDocs1CUPP);
                unmatchedDocsCommentator.CommentUnmatchedDocuments();
            }


            void GenerateOutputSpreadsheets()
            {
                SpreadSheetWriterXLSX spreadsheetWriterPassedDo;                
                SpreadSheetWriterXLSX spreadSheetWriterPassedUpp;               

                spreadsheetWriterPassedDo = new(workParams.outputUnmatchDocManagePath);
                spreadsheetWriterPassedDo.CreateSpreadsheet(docComparator.UnmatchedDocs1CDO);

                spreadSheetWriterPassedUpp = new(workParams.outputUnmatchedDocRegistryPath);
                spreadSheetWriterPassedUpp.CreateSpreadsheet(docComparator.UnmatchedDocs1CUPP, false);

                if (workParams.isPrintMatchedDocuments)
                {
                    SpreadSheetWriterXLSX spreadSheetWriterMatchedDo;               
                    SpreadSheetWriterXLSX spreadSheetWriterMatcheUpp;               

                    spreadSheetWriterMatchedDo = new(workParams.outputMatchedDocManagePath);
                    spreadSheetWriterMatchedDo.CreateSpreadsheet(docComparator.MatchedDocs1CDO, false);

                    spreadSheetWriterMatcheUpp = new(workParams.outputMatchedDocRestryPath);
                    spreadSheetWriterMatcheUpp.CreateSpreadsheet(docComparator.MatchedDocs1CUPP, false);
                }
            }


            void GetSrcDocsAmount()
            {
                reportDocAmount.doDocumentsCount = docs1CDO.Count;
                reportDocAmount.uppDocumentsCount = docsRegistry.Count;
                reportDocAmount.ignoreDoDocumentsCount = passDocs1CDO.Count;
                reportDocAmount.ignoreUppDocumentsCount = passDocsRegistry.Count;
            }


            void GetResultDocsAmount()
            {
                reportDocAmount.Documents1CDoUnmatchedCount = docComparator.UnmatchedDocs1CDO.Count;
                reportDocAmount.Documents1CUppUnmatchedCount = docComparator.UnmatchedDocs1CUPP.Count;
                reportDocAmount.Documents1CDoMatchedCount = docComparator.MatchedDocs1CDO.Count;
                reportDocAmount.Documents1CUppMatchedCount = docComparator.MatchedDocs1CUPP.Count;
            }


            void CloseProgram()
            {
                if (workParams.isAskAboutCloseProgram)
                {
                    Console.WriteLine("Для завершения программы нажмите любую клавишу.");
                    Console.ReadKey();
                }
            }
        }
    }
}