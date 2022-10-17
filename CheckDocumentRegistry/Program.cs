
namespace RegComparator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            List<Document> doDocuments;                         // Source 1C:DO documents for compare
            List<Document> regDocuments;                        // Source 1C:UPP documents for compare


            List<Document> kaDocumentsSf;                        
            List<Document> kaDocumentsTn;
            List<Document> kaDocumentsAll;

            List<Document> ignoreDoDocuments;                   // Ignore list of 1C:DO documents 
            List<Document> ignoreRegDocuments;                  // Ignore list of 1C:UPP documents
            FullDocComparator compareResult;                    // Class contains results of documents comparing 
            UnmatchedDocCommentSetter unmatchedDocumentsCommentator;  // Class set comments in unmatched documents
            SpreadSheetWriterXLSX spreadSheetWriterPassedDo;    // Spreadsheet creator
            SpreadSheetWriterXLSX spreadSheetWriterPassedUpp;   // Spreadsheet creator
            SpreadSheetWriterXLSX spreadSheetWriterMatchedDo;   // Spreadsheet creator
            SpreadSheetWriterXLSX spreadSheetWriterMatcheUpp;   // Spreadsheet creator
            InternalParameters internalParameters;              // Static parameters
            UserParameters userParameters;                      // Loaded from config file parameters
            DocLoader documentsLoader;
            DocumentsAmount documentsAmount = new();

            GetProgramParameters(args);                             // Getting program parameters

            //WorkAbilityChecker.CheckFiles(userParameters);   // Checkimg for existing files to comparing

            if (userParameters.programMode == "UPP")
            {
                GetSourseDocuments1CDO();
                GetSourseDocuments1CUPP();                               // Getting documents from spreadsheets
                GetIgnoreLists();                          // Getting ignored documents from spreadsheets
                GetSourceDocumentsCounts();
                CompareDocuments();
                GetDocumentsAmounts();
                DocumentAmountReporter documentsAmountReporter = new(userParameters.reportFilePath);
                documentsAmountReporter.CreateAllReports(regDocuments, documentsAmount);
                GenerateOutputSpreadsheets();
            }

            if (userParameters.programMode == "KA")
            {
                GetSourseDocuments1CDO();
                GetSourseDocuments1CKA();
                GetIgnoreLists();                          // Getting ignored documents from spreadsheets
                GetSourceDocumentsCounts();
                CompareDocuments();
                GetDocumentsAmounts();
                DocumentAmountReporter documentsAmountReporter = new(userParameters.reportFilePath);
                documentsAmountReporter.CreateAllReports(regDocuments, documentsAmount);
                GenerateOutputSpreadsheets();
            }


                // Creating reports

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
                return;
            }

            void GetSourseDocuments1CDO()
            {
                documentsLoader = new();
                doDocuments = documentsLoader.GetDocs1CDO(userParameters.doSpreadSheetPath,
                                                               userParameters.exceptedDoPath);
            }


                void GetSourseDocuments1CUPP()
            {
                documentsLoader = new();
                regDocuments = documentsLoader.GetDocs1CUPP(userParameters.uppSpreadSheetPath,
                                                               userParameters.exceptedUppPath);
            }

            void GetSourseDocuments1CKA()
            {
                documentsLoader = new();
                kaDocumentsSf = documentsLoader.GetDocs1CKASf(userParameters.kaSpreadsheetSf,
                                                               userParameters.exceptSpreadsheet1CKASf);

                kaDocumentsTn = documentsLoader.GetDocs1CKATn(userParameters.kaSpreadsheetTn,
                                                               userParameters.exceptSpreadsheet1CKATn);

                regDocuments = kaDocumentsSf.Concat(kaDocumentsTn).ToList();

            }


            void GetIgnoreLists()
            {
                ignoreDoDocuments = documentsLoader.GetDocsPass(userParameters.doIgnoreSpreadSheetPath);
                ignoreRegDocuments = documentsLoader.GetDocsPass(userParameters.uppIgnoreSpreadSheetPath);
            }


            void CompareDocuments()
            {
                compareResult = new(doDocuments, regDocuments, ignoreDoDocuments, ignoreRegDocuments);
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
                documentsAmount.uppDocumentsCount = regDocuments.Count;
                documentsAmount.ignoreDoDocumentsCount = ignoreDoDocuments.Count;
                documentsAmount.ignoreUppDocumentsCount = ignoreRegDocuments.Count;
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