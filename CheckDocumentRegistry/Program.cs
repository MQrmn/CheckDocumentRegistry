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
            IDocComparator docComparator;
            IUnmatchedDocMarker unmatchedDocMarker;

            // Repositories
            ProgramParamsRepositoryBase progParamsRepo;                             // Contains program parameters
            SpreadsheetsPathsBase spreadsheetsPaths1CDO, spreadsheetsPathsRegistry; // Contains spreadsheets paths in file system
            FieldsSettingsRepositoryBase fieldsSettings1CDO, fieldsSettingsRegistry;
            DocRepositoryBase docRepo1CDO, docRepoRegistry;
            
            // CREATING INSTANCES
            // Common utils
            userReporter = new ConsoleWriter();
            objectConverter = new ReadWriteJSON();
            objectConverter.ErrNotify += userReporter.ReportError;
            fileExistChecker = new FileExistChecker();
            fileExistChecker.ErrNotify += userReporter.ReportError;

            // Set program patameters
            argsHandler = new ArgsHandler(args);
            progParamsRepo = new ProgramParamsRepository(argsHandler.GetParams(), objectConverter, fileExistChecker);
            progParamsRepo.Notify += userReporter.ReportInfo;
            progParamsRepo.ErrNotify += userReporter.ReportError;
            progParamsRepo.FillRepository();

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

            docComparator = new DocComparator(docRepo1CDO, docRepoRegistry);
            docComparator.CompareDocuments();

            // Mark unmatched docs
            unmatchedDocMarker = new UnmatchedDocMarker(docRepo1CDO.UnmatchedDocs, docRepoRegistry.UnmatchedDocs);
            unmatchedDocMarker.MarkDocuments();

            FillSrcDocAmount();
            // Comparing documents
            FillResultDocAmount();
            
            DocumentAmountReporter documentsAmountReporter = new(progParamsRepo.Common.ProgramReportFilePath);
            documentsAmountReporter.CreateAllReports(docRepoRegistry.SourceDocs,reportDocAmount);
            // Result spreadsheets generating
            GenerateOutputSpreadsheets();
            CloseProgram();

            void FillSrcDocAmount()
            {
                reportDocAmount.doDocumentsCount = docRepo1CDO.SourceDocs.Count;
                reportDocAmount.uppDocumentsCount = docRepoRegistry.SourceDocs.Count;
                reportDocAmount.ignoreDoDocumentsCount = docRepo1CDO.SkippedDocs.Count;
                reportDocAmount.ignoreUppDocumentsCount = docRepoRegistry.SkippedDocs.Count;
            }

            void FillResultDocAmount()
            {
                reportDocAmount.Documents1CDoUnmatchedCount = docRepo1CDO.UnmatchedDocs.Count;
                reportDocAmount.Documents1CUppUnmatchedCount = docRepoRegistry.UnmatchedDocs.Count;
                reportDocAmount.Documents1CDoMatchedCount = docRepo1CDO.MatchedDocs.Count;
                reportDocAmount.Documents1CUppMatchedCount = docRepoRegistry.MatchedDocs.Count;
            }

            void GenerateOutputSpreadsheets()
            {
                SpreadSheetWriterXLSX spreadsheetWriterPassedDo;                
                SpreadSheetWriterXLSX spreadSheetWriterPassedUpp;               

                spreadsheetWriterPassedDo = new(progParamsRepo.Spreadsheets1CDO.Unmatched);
                spreadsheetWriterPassedDo.CreateSpreadsheet(docRepo1CDO.UnmatchedDocs);

                spreadSheetWriterPassedUpp = new(progParamsRepo.SpreadsheetsRegistry.Unmatched);
                spreadSheetWriterPassedUpp.CreateSpreadsheet(docRepoRegistry.UnmatchedDocs, false);

                if (progParamsRepo.Common.IsPrintMatchedDocuments)
                {
                    SpreadSheetWriterXLSX spreadSheetWriterMatchedDo;               
                    SpreadSheetWriterXLSX spreadSheetWriterMatcheUpp;               

                    spreadSheetWriterMatchedDo = new(progParamsRepo.Spreadsheets1CDO.Matched);
                    spreadSheetWriterMatchedDo.CreateSpreadsheet(docRepo1CDO.MatchedDocs, false);

                    spreadSheetWriterMatcheUpp = new(progParamsRepo.SpreadsheetsRegistry.Matched);
                    spreadSheetWriterMatcheUpp.CreateSpreadsheet(docRepoRegistry.MatchedDocs, false);
                }
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