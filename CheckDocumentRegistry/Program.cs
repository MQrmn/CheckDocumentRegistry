namespace RegComparator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Utils
            IUserReporter userReporter;                                             // Reports providing
            IObjsConverter objectConverter;                                         // Objects Reader-Writer in/to file
            ISpreadSheetReader spreadSheetReader;                                   // Getting data from spreadsheetd
            IArrToObjConverter arrToObjConverter;                                   // Getting objs from file, putting objs to file
            IFileExistChecker fileExistChecker;
            IDocLoader docLoader;
            IDocRepositoryFiller docRepoFiller1CDO, docRepoFillerRegidtry;
            IArgsHandler argsHandler;

            // Repositories
            ProgramParamsRepositoryBase progParamsRepo;                             // Contains programs parameters
            SpreadsheetsPathsBase spreadsheetsPaths1CDO, spreadsheetsPathsRegistry; // Contains spreadsheets paths in file system
            FieldsSettingsRepositoryBase fieldsSettings1CDO, fieldsSettingsRegistry;
            DocRepositoryBase docRepo1CDO, docRepoRegistry;
            
            // CREATING INSTANCES
            // Common utils
            userReporter = new ConsoleWriter();
            objectConverter = new ReadWriteJSON();
            objectConverter.ErrNotify += userReporter.ReportError;
            fileExistChecker = new FileExistChecker();
            
            // Set program patameters
            argsHandler = new ArgsHandler(args);
            progParamsRepo = new ProgramParamsRepository(argsHandler.GetParams(), objectConverter, fileExistChecker);

            // Creating docprocessors
            spreadSheetReader = new SpreadSheetReaderXLSX();
            arrToObjConverter = new ArrToObjConverter();
            arrToObjConverter.ErrNotify += userReporter.ReportError;
            docLoader = new DocLoader(arrToObjConverter, spreadSheetReader);
            docLoader.Notify += userReporter.ReportInfo;

            // Creating document fields settings repositories
            fieldsSettings1CDO = new FieldsSettings1CDORepository();
            fieldsSettingsRegistry = new FieldsSettingsRegistryRepository();

            // Creating Documents repositories
            docRepo1CDO = new DocRepository1CDO();
            docRepoRegistry = new DocRepositoryRegistry();

            DocAmountReportData reportDocAmount = new();  // NOT REFACTORED

            // Creating documents repositories fillers
            docRepoFiller1CDO     = new DocRepositoryFiller(docLoader, 
                                        fieldsSettings1CDO, 
                                        docRepo1CDO,
                                        progParamsRepo.Spreadsheets1CDO
                                        );
            docRepoFillerRegidtry = new DocRepositoryFiller(docLoader,
                                        fieldsSettingsRegistry,
                                        docRepoRegistry,
                                        progParamsRepo.SpreadsheetsRegistry
                                        );
            // Filling doc repositories 
            docRepoFiller1CDO.FillRepository();
            docRepoFillerRegidtry.FillRepository();

            // NOT REFACTORED
            DocComparator docComparator;
            UnmatchedDocCommentSetter unmatchedDocsCommentator;

            FillSrcDocAmount();
            // Comparing documents
            CompareDocuments();
            FillResultDocAmount();
            
            DocumentAmountReporter documentsAmountReporter = new(progParamsRepo.Common.ProgramReportFilePath);
            documentsAmountReporter.CreateAllReports(
                                                    docRepoRegistry.SourceDocs,
                                                    reportDocAmount
                                                    );
            // Result spreadsheets generating
            GenerateOutputSpreadsheets();
            CloseProgram();

            void CompareDocuments()
            {
                docComparator = new(docRepo1CDO.SourceDocs,
                                    docRepoRegistry.SourceDocs,
                                    docRepoRegistry.SkippedDocs,
                                    docRepoRegistry.SkippedDocs
                                    );
                unmatchedDocsCommentator = new(docComparator.UnmatchedDocs1CDO,
                                               docComparator.UnmatchedDocs1CUPP);
                unmatchedDocsCommentator.CommentUnmatchedDocuments();
            }

            void GenerateOutputSpreadsheets()
            {
                SpreadSheetWriterXLSX spreadsheetWriterPassedDo;                
                SpreadSheetWriterXLSX spreadSheetWriterPassedUpp;               

                spreadsheetWriterPassedDo = new(progParamsRepo.Spreadsheets1CDO.Unmatched);
                spreadsheetWriterPassedDo.CreateSpreadsheet(docComparator.UnmatchedDocs1CDO);

                spreadSheetWriterPassedUpp = new(progParamsRepo.SpreadsheetsRegistry.Unmatched);
                spreadSheetWriterPassedUpp.CreateSpreadsheet(docComparator.UnmatchedDocs1CUPP, false);

                if (progParamsRepo.Common.IsPrintMatchedDocuments)
                {
                    SpreadSheetWriterXLSX spreadSheetWriterMatchedDo;               
                    SpreadSheetWriterXLSX spreadSheetWriterMatcheUpp;               

                    spreadSheetWriterMatchedDo = new(progParamsRepo.Spreadsheets1CDO.Matched);
                    spreadSheetWriterMatchedDo.CreateSpreadsheet(docComparator.MatchedDocs1CDO, false);

                    spreadSheetWriterMatcheUpp = new(progParamsRepo.SpreadsheetsRegistry.Matched);
                    spreadSheetWriterMatcheUpp.CreateSpreadsheet(docComparator.MatchedDocs1CUPP, false);
                }
            }

            void FillSrcDocAmount()
            {
                reportDocAmount.doDocumentsCount = docRepo1CDO.SourceDocs.Count;
                reportDocAmount.uppDocumentsCount = docRepoRegistry.SourceDocs.Count;
                reportDocAmount.ignoreDoDocumentsCount = docRepo1CDO.SkippedDocs.Count;
                reportDocAmount.ignoreUppDocumentsCount = docRepoRegistry.SkippedDocs.Count;
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
                if (progParamsRepo.Common.IsAskAboutCloseProgram)
                {
                    Console.WriteLine("Для завершения программы нажмите любую клавишу.");
                    Console.ReadKey();
                }
            }
        }
    }
}