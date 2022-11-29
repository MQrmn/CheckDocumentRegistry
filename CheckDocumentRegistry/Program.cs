
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
            ConfigFilesPath configFilesPath;                                // Static parameters
            DocLoader docLoader;

            DocAmount docAmount = new();
            WorkParams workParams = GetParams(args);                        // Getting program parameters
            WorkAbilityChecker.CheckFiles(workParams);                      // Checkimg for existing files to comparing & access to them
            
            GetSrcDocs1CDO();                                               // Getting 1C:DO src documents from spreadsheets
            
            if (workParams.programMode == "UPP")                            // Getting Registry documents from spreadsheets
                GetSrcDocs1CUPP();                                          
            else
                GetSrcDocs1CKA();

            GetIgnoreDocList();                                             // Getting ignored documents from spreadsheets
            GetSrcDocsAmount();
            CompareDocuments();                                             // Comparing
            GetResultDocsAmount();

            DocumentAmountReporter documentsAmountReporter = new(workParams.programReportFilePath);
            documentsAmountReporter.CreateAllReports(docsRegistry, docAmount);

            // Result spreadsheets generating
            GenerateOutputSpreadsheets();
            CloseProgram();


            void ArgsHandler(string[] args)
            { 
                if(args.Length < 2)
                    return;
                if (args[0] == "-c")
                {
                    configFilesPath.ProgramParametersFilePath = args[1];
                }
            }

            WorkParams GetParams(string[] args)
            {
                configFilesPath = new();
                ArgsHandler(args);
                WorkParametersReadWrite workParametersReadWrite = new(configFilesPath.ProgramParametersFilePath);
                return workParametersReadWrite.GetProgramParameters();
            }

            void GetSrcDocs1CDO()
            {
                docLoader = new();
                docs1CDO = docLoader
                    .GetDocs1CDO(workParams.inputSpreadsheetDocManagePath,
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
                passDocs1CDO = docLoader.GetDocsPass(workParams.passSpreadsheetDocManagePath);          // ? Pass or Ignore documents?
                passDocsRegistry = docLoader.GetDocsPass(workParams.passSpreadSheetDocRegistryPath);    // ? Pass or Ignore documents?
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

                spreadsheetWriterPassedDo = new(workParams.outputUnmatchDocManagePath);         // ? Why is not enough single instanse of XLSXwriter ?
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
                docAmount.doDocumentsCount = docs1CDO.Count;
                docAmount.uppDocumentsCount = docsRegistry.Count;
                docAmount.ignoreDoDocumentsCount = passDocs1CDO.Count;
                docAmount.ignoreUppDocumentsCount = passDocsRegistry.Count;
            }


            void GetResultDocsAmount()
            {
                docAmount.Documents1CDoUnmatchedCount = docComparator.UnmatchedDocs1CDO.Count;
                docAmount.Documents1CUppUnmatchedCount = docComparator.UnmatchedDocs1CUPP.Count;
                docAmount.Documents1CDoMatchedCount = docComparator.MatchedDocs1CDO.Count;
                docAmount.Documents1CUppMatchedCount = docComparator.MatchedDocs1CUPP.Count;
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