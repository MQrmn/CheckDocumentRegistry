
namespace RegComparator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DocRepositoryBase docRepository;
            List<Document> docs1CDO;                                        // Source 1C:DO documents for compare
            List<Document> docsRegistry;                                    // Source 1C:UPP, 1C:KA documents for compare
            List<Document> passDocs1CDO;                                    // Ignore list of 1C:DO documents 
            List<Document> passDocsRegistry;                                // Ignore list of 1C:UPP documents
            DocComparator docComparator;                                    // Class contains results of documents comparing 
            UnmatchedDocCommentSetter unmatchedDocsCommentator;             // Class set comments in unmatched documents
            ConfigFilesPath configFilesPath;                                
            DocLoader docLoader;

            DocAmountReportData reportDocAmount = new();

            WorkParams workParams = GetWorkParams(args);                    // Getting program parameters
            WorkAbilityChecker.CheckFiles(workParams);                      // Checkimg for existing files to comparing

            GetSrcDocs1CDO();                                               // Getting documents from 1C:DO
            GetRegistryDocs();                                              // Getting documents from 1C:RF or UPP registry
            GetIgnoreDocList();                                             // Getting ignored documents from spreadsheets
            FillSrcDocAmount();
            CompareDocuments();                                             // Comparing
            FillResultDocAmount();

            DocumentAmountReporter documentsAmountReporter = new(workParams.programReportFilePath);
            documentsAmountReporter.CreateAllReports(docsRegistry, reportDocAmount);

            // Result spreadsheets generating
            GenerateOutputSpreadsheets();
            CloseProgram();

            void GetSrcDocs1CDO()
            {
                docLoader = new();
                docs1CDO = docLoader.GetDocs1CDO(workParams.inputSpreadsheetDocManagePath,
                                                               workParams.exceptedDocManagePath);
            }

            void GetRegistryDocs(){
                if (workParams.programMode == "UPP")
                    GetSrcDocs1CUPP();
                else
                    GetSrcDocs1CKA();
            }

            void GetSrcDocs1CUPP()
            {
                docLoader = new();
                docsRegistry = docLoader.GetDocs1CUPP(workParams.inputSpreadsheetDocRegistryPath[0],
                                                               workParams.exceptedDocRegistryPath);
            }

            void GetSrcDocs1CKA()
            {
                List<Document> documentsRegistryKaPart1;    // ?! What is a parts? 
                List<Document> documentsRegistryKaPart2;    // ?! What is a parts? 
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

            void ArgsHandler(string[] args)
            { 
                if(args.Length < 2)
                    return;
                if (args[0] == "-c")
                {
                    configFilesPath.ProgramParametersFilePath = args[1];
                }
            }

            WorkParams GetWorkParams(string[] args)
            {
                configFilesPath = new();
                ArgsHandler(args);
                WorkParametersReadWrite workParametersReadWrite = new(configFilesPath.ProgramParametersFilePath);
                return workParametersReadWrite.GetProgramParameters();
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

            void FillSrcDocAmount()
            {
                reportDocAmount.doDocumentsCount = docs1CDO.Count;
                reportDocAmount.uppDocumentsCount = docsRegistry.Count;
                reportDocAmount.ignoreDoDocumentsCount = passDocs1CDO.Count;
                reportDocAmount.ignoreUppDocumentsCount = passDocsRegistry.Count;
            }

            void FillResultDocAmount()
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