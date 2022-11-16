
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

            DocsAmount docsAmount = new();
            WorkParameters programParameters = GetProgramParams(args);      // Getting program parameters
            WorkAbilityChecker.CheckFiles(programParameters);               // Checkimg for existing files to comparing

            GetSrcDocs1CDO();

            if (programParameters.programMode == "UPP")
                GetSrcDocs1CUPP();                                          // Getting documents from spreadsheets
            else
                GetSrcDocs1CKA();

            GetIgnoreDocList();                                             // Getting ignored documents from spreadsheets
            GetSrcDocsAmount();
            CompareDocuments();
            GetResultDocsAmount();

            DocumentAmountReporter documentsAmountReporter = new(programParameters.programReportFilePath);
            documentsAmountReporter.CreateAllReports(docsRegistry, docsAmount);

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


            WorkParameters GetProgramParams(string[] args)
            {
                internalParameters = new();
                ArgsHandler(args);
                ProgramParametersReadWrite programParameters1 = new(internalParameters.ProgramParametersFilePath);
                return programParameters1.GetProgramParameters();
            }


            void GetSrcDocs1CDO()
            {
                docLoader = new();
                docs1CDO = docLoader.GetDocs1CDO(programParameters.inputSpreadsheetDocManagePath,
                                                               programParameters.exceptedDocManagePath);
            }


            void GetSrcDocs1CUPP()
            {
                docLoader = new();
                docsRegistry = docLoader.GetDocs1CUPP(programParameters.inputSpreadsheetDocRegistryPath[0],
                                                               programParameters.exceptedDocRegistryPath);
            }

            void GetSrcDocs1CKA()
            {
                List<Document> documentsRegistryKaPart1;
                List<Document> documentsRegistryKaPart2;
                docLoader = new();
                documentsRegistryKaPart1 = docLoader.GetDocs1CKASf(programParameters.inputSpreadsheetDocRegistryPath[0],
                                                               programParameters.exceptedDocManagePath);

                documentsRegistryKaPart2 = docLoader.GetDocs1CKATn(programParameters.inputSpreadsheetDocRegistryPath[1],
                                                               programParameters.exceptedDocManagePath);

                docsRegistry = documentsRegistryKaPart1.Concat(documentsRegistryKaPart2).ToList();
            }


            void GetIgnoreDocList()
            {
                passDocs1CDO = docLoader.GetDocsPass(programParameters.passSpreadsheetDocManagePath);
                passDocsRegistry = docLoader.GetDocsPass(programParameters.passSpreadSheetDocRegistryPath);
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

                spreadsheetWriterPassedDo = new(programParameters.outputUnmatchDocManagePath);
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
                docsAmount.doDocumentsCount = docs1CDO.Count;
                docsAmount.uppDocumentsCount = docsRegistry.Count;
                docsAmount.ignoreDoDocumentsCount = passDocs1CDO.Count;
                docsAmount.ignoreUppDocumentsCount = passDocsRegistry.Count;
            }


            void GetResultDocsAmount()
            {
                docsAmount.Documents1CDoUnmatchedCount = docComparator.UnmatchedDocs1CDO.Count;
                docsAmount.Documents1CUppUnmatchedCount = docComparator.UnmatchedDocs1CUPP.Count;
                docsAmount.Documents1CDoMatchedCount = docComparator.MatchedDocs1CDO.Count;
                docsAmount.Documents1CUppMatchedCount = docComparator.MatchedDocs1CUPP.Count;
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