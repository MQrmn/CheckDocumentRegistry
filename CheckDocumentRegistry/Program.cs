
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

<<<<<<< HEAD
            DocAmount docAmount = new();
            WorkParams workParams = GetParams(args);                        // Getting program parameters
            WorkAbilityChecker.CheckFiles(workParams);                      // Checkimg for existing files to comparing & access to them
            
            GetSrcDocs1CDO();                                               // Getting 1C:DO src documents from spreadsheets
            
            if (workParams.programMode == "UPP")                            // Getting Registry documents from spreadsheets
                GetSrcDocs1CUPP();                                          
=======
            DocsAmount docsAmount = new();
            WorkParameters programParameters = GetProgramParams(args);      // Getting program parameters
            WorkAbilityChecker.CheckFiles(programParameters);               // Checkimg for existing files to comparing

            GetSrcDocs1CDO();

            if (programParameters.programMode == "UPP")
                GetSrcDocs1CUPP();                                          // Getting documents from spreadsheets
>>>>>>> parent of e7b3b6c (Added ArgsHandler)
            else
                GetSrcDocs1CKA();

            GetIgnoreDocList();                                             // Getting ignored documents from spreadsheets
            GetSrcDocsAmount();
            CompareDocuments();                                             // Comparing
            GetResultDocsAmount();

<<<<<<< HEAD
            DocumentAmountReporter documentsAmountReporter = new(workParams.programReportFilePath);
            documentsAmountReporter.CreateAllReports(docsRegistry, docAmount);
=======
            DocumentAmountReporter documentsAmountReporter = new(programParameters.programReportFilePath);
            documentsAmountReporter.CreateAllReports(docsRegistry, docsAmount);
>>>>>>> parent of e7b3b6c (Added ArgsHandler)

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

<<<<<<< HEAD
            WorkParams GetParams(string[] args)
=======

            WorkParameters GetProgramParams(string[] args)
>>>>>>> parent of e7b3b6c (Added ArgsHandler)
            {
                configFilesPath = new();
                ArgsHandler(args);
                WorkParametersReadWrite workParametersReadWrite = new(configFilesPath.ProgramParametersFilePath);
                return workParametersReadWrite.GetProgramParameters();
            }

            void GetSrcDocs1CDO()
            {
                docLoader = new();
<<<<<<< HEAD
                docs1CDO = docLoader
                    .GetDocs1CDO(workParams.inputSpreadsheetDocManagePath,
                                 workParams.exceptedDocManagePath);
=======
                docs1CDO = docLoader.GetDocs1CDO(programParameters.inputSpreadsheetDocManagePath,
                                                               programParameters.exceptedDocManagePath);
>>>>>>> parent of e7b3b6c (Added ArgsHandler)
            }

            void GetSrcDocs1CUPP()
            {
                docLoader = new();
                docsRegistry = docLoader.GetDocs1CUPP(programParameters.inputSpreadsheetDocRegistryPath[0],
                                                               programParameters.exceptedDocRegistryPath);
            }

            void GetSrcDocs1CKA()
            {
                List<Document> documentsRegistryKaPart1;    // ?! What is a parts? 
                List<Document> documentsRegistryKaPart2;    // ?! What is a parts? 
                docLoader = new();
                documentsRegistryKaPart1 = docLoader.GetDocs1CKASf(programParameters.inputSpreadsheetDocRegistryPath[0],
                                                               programParameters.exceptedDocManagePath);

                documentsRegistryKaPart2 = docLoader.GetDocs1CKATn(programParameters.inputSpreadsheetDocRegistryPath[1],
                                                               programParameters.exceptedDocManagePath);

                docsRegistry = documentsRegistryKaPart1.Concat(documentsRegistryKaPart2).ToList();
            }

            void GetIgnoreDocList()
            {
<<<<<<< HEAD
                passDocs1CDO = docLoader.GetDocsPass(workParams.passSpreadsheetDocManagePath);          // ? Pass or Ignore documents?
                passDocsRegistry = docLoader.GetDocsPass(workParams.passSpreadSheetDocRegistryPath);    // ? Pass or Ignore documents?
=======
                passDocs1CDO = docLoader.GetDocsPass(programParameters.passSpreadsheetDocManagePath);
                passDocsRegistry = docLoader.GetDocsPass(programParameters.passSpreadSheetDocRegistryPath);
>>>>>>> parent of e7b3b6c (Added ArgsHandler)
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

<<<<<<< HEAD
                spreadsheetWriterPassedDo = new(workParams.outputUnmatchDocManagePath);         // ? Why is not enough single instanse of XLSXwriter ?
=======
                spreadsheetWriterPassedDo = new(programParameters.outputUnmatchDocManagePath);
>>>>>>> parent of e7b3b6c (Added ArgsHandler)
                spreadsheetWriterPassedDo.CreateSpreadsheet(docComparator.UnmatchedDocs1CDO);

                spreadSheetWriterPassedUpp = new(programParameters.outputUnmatchedDocRegistryPath);
                spreadSheetWriterPassedUpp.CreateSpreadsheet(docComparator.UnmatchedDocs1CUPP, false);

                if (programParameters.isPrintMatchedDocuments)
                {
                    SpreadSheetWriterXLSX spreadSheetWriterMatchedDo;               
                    SpreadSheetWriterXLSX spreadSheetWriterMatcheUpp;               

                    spreadSheetWriterMatchedDo = new(programParameters.outputMatchedDocManagePath);
                    spreadSheetWriterMatchedDo.CreateSpreadsheet(docComparator.MatchedDocs1CDO, false);

                    spreadSheetWriterMatcheUpp = new(programParameters.outputMatchedDocRestryPath);
                    spreadSheetWriterMatcheUpp.CreateSpreadsheet(docComparator.MatchedDocs1CUPP, false);
                }
            }


            void GetSrcDocsAmount()
            {
<<<<<<< HEAD
                docAmount.doDocumentsCount = docs1CDO.Count;
                docAmount.uppDocumentsCount = docsRegistry.Count;
                docAmount.ignoreDoDocumentsCount = passDocs1CDO.Count;
                docAmount.ignoreUppDocumentsCount = passDocsRegistry.Count;
=======
                docsAmount.doDocumentsCount = docs1CDO.Count;
                docsAmount.uppDocumentsCount = docsRegistry.Count;
                docsAmount.ignoreDoDocumentsCount = passDocs1CDO.Count;
                docsAmount.ignoreUppDocumentsCount = passDocsRegistry.Count;
>>>>>>> parent of e7b3b6c (Added ArgsHandler)
            }


            void GetResultDocsAmount()
            {
<<<<<<< HEAD
                docAmount.Documents1CDoUnmatchedCount = docComparator.UnmatchedDocs1CDO.Count;
                docAmount.Documents1CUppUnmatchedCount = docComparator.UnmatchedDocs1CUPP.Count;
                docAmount.Documents1CDoMatchedCount = docComparator.MatchedDocs1CDO.Count;
                docAmount.Documents1CUppMatchedCount = docComparator.MatchedDocs1CUPP.Count;
=======
                docsAmount.Documents1CDoUnmatchedCount = docComparator.UnmatchedDocs1CDO.Count;
                docsAmount.Documents1CUppUnmatchedCount = docComparator.UnmatchedDocs1CUPP.Count;
                docsAmount.Documents1CDoMatchedCount = docComparator.MatchedDocs1CDO.Count;
                docsAmount.Documents1CUppMatchedCount = docComparator.MatchedDocs1CUPP.Count;
>>>>>>> parent of e7b3b6c (Added ArgsHandler)
            }


            void CloseProgram()
            {
                if (programParameters.isAskAboutCloseProgram)
                {
                    Console.WriteLine("Для завершения программы нажмите любую клавишу.");
                    Console.ReadKey();
                }
            }
        }
    }
}