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
            IDocAmountsReporter docAmountsReporter;
            ISpreadSheetWriterXLSX spWriter;

            // Repositories
            ProgramParamsRepositoryBase progParamsRepo;                             // Contains program parameters
            SpreadsheetsPathsBase spreadsheetsPaths1CDO, spreadsheetsPathsRegistry; // Contains spreadsheets paths in file system
            FieldsSettingsRepositoryBase fieldsSettings1CDO, fieldsSettingsRegistry;
            DocRepositoryBase docRepo1CDO, docRepoRegistry;
            DocAmountsRepositoryBase docAmounts;
            
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

            // Documents comparing
            docComparator = new DocComparator(docRepo1CDO, docRepoRegistry);
            docComparator.CompareDocuments();

            // Mark unmatched docs
            unmatchedDocMarker = new UnmatchedDocMarker(docRepo1CDO.UnmatchedDocs, docRepoRegistry.UnmatchedDocs);
            unmatchedDocMarker.MarkDocuments();
            
            // Report generating
            docAmounts = new DocAmountsRepository(new DocAmounts(docRepo1CDO), new DocAmounts(docRepoRegistry));
            docAmountsReporter = new DocAmountsReporter(docAmounts, docRepoRegistry);
            docAmountsReporter.Notify += userReporter.ReportSpecial;
            docAmountsReporter.CreateReport();

            // Result spreadsheets generating
            GenerateOutputSpreadsheets();
            CloseProgram();

            void GenerateOutputSpreadsheets()
            {
                spWriter = new SpreadSheetWriterXLSX(progParamsRepo.Spreadsheets1CDO.Unmatched);
                spWriter.CreateSpreadsheet(docRepo1CDO.UnmatchedDocs);
                spWriter = new SpreadSheetWriterXLSX(progParamsRepo.SpreadsheetsRegistry.Unmatched);
                spWriter.CreateSpreadsheet(docRepoRegistry.UnmatchedDocs, false);

                if (progParamsRepo.Common.IsPrintMatchedDocuments)
                {
                    spWriter = new SpreadSheetWriterXLSX(progParamsRepo.Spreadsheets1CDO.Matched);
                    spWriter.CreateSpreadsheet(docRepo1CDO.MatchedDocs, false);
                    spWriter = new SpreadSheetWriterXLSX(progParamsRepo.SpreadsheetsRegistry.Matched);
                    spWriter.CreateSpreadsheet(docRepoRegistry.MatchedDocs, false);
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