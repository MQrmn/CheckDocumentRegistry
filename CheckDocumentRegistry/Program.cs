namespace RegComparator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Utils
            IUserReporter consoleWriter;                                             // Reports providing
            IUserReporter fileWriter;
            IObjsConverter objFileConverter;                                         // Objects Reader-Writer in/to file
            ISpreadSheetReader spsReader;                                            // Getting data from spreadsheetd
            IArrToObjConverter arrToObjConverter;                                    // Getting objs from file, putting objs to file
            IFileExistChecker fileExistChecker;
            IDocLoader docLoader;
            IDocRepositoryFiller docRepoFiller1CDO, docRepoFillerRegidtry;
            IArgsHandler argsHandler;
            IDocComparator docComparator;
            IUnmatchedDocMarker unmatchedDocMarker;
            IDocAmountsReporter docAmountsReporter;
            ISpreadSheetWriterXLSX spsWriter;
            

            // Repositories
            ProgramParamsRepositoryBase progParamsRepo;                             // Contains program parameters
            SpreadsheetsPathsBase spreadsheetsPaths1CDO, spreadsheetsPathsRegistry; // Contains spreadsheets paths in file system
            FieldsSettingsRepositoryBase fieldsSettings1CDO, fieldsSettingsRegistry;
            DocRepositoryBase docRepo1CDO, docRepoRegistry;
            DocAmountsRepositoryBase docAmounts;
            
            // CREATING INSTANCES
            // Create common utils
            consoleWriter = new ConsoleWriter();
            
            objFileConverter = new ReadWriteJSON();
            objFileConverter.ErrNotify += consoleWriter.ReportError;
            fileExistChecker = new FileExistChecker();
            fileExistChecker.ErrNotify += consoleWriter.ReportError;

            // Set program patameters
            argsHandler = new ArgsHandler(args);
            progParamsRepo = new ProgramParamsRepository(argsHandler.GetParams(), objFileConverter, fileExistChecker);
            progParamsRepo.Notify += consoleWriter.ReportInfo;
            progParamsRepo.ErrNotify += consoleWriter.ReportError;
            progParamsRepo.FillRepository();

            // Create docprocessors
            spsReader = new SpreadSheetReaderXLSX();
            arrToObjConverter = new ArrToObjConverter();
            arrToObjConverter.ErrNotify += consoleWriter.ReportError;
            docLoader = new DocLoader(arrToObjConverter, spsReader);
            docLoader.Notify += consoleWriter.ReportInfo;

            // Create document fields settings repositories
            fieldsSettings1CDO = new FieldsSettings1CDORepository();
            fieldsSettingsRegistry = new FieldsSettingsRegistryRepository();

            // Create documents repositories
            docRepo1CDO = new DocRepository1CDO();
            docRepoRegistry = new DocRepositoryRegistry();

            // Filling doc repositories
            docRepoFiller1CDO = new DocRepositoryFiller(docLoader, 
                                                            fieldsSettings1CDO, 
                                                            docRepo1CDO,
                                                            progParamsRepo.Spreadsheets1CDO
                                                            );
            docRepoFillerRegidtry = new DocRepositoryFiller(docLoader,
                                                            fieldsSettingsRegistry,
                                                            docRepoRegistry,
                                                            progParamsRepo.SpreadsheetsRegistry
                                                            );

            docRepoFiller1CDO.FillRepository();
            docRepoFillerRegidtry.FillRepository();

            // Compare documents
            docComparator = new DocComparator(docRepo1CDO, docRepoRegistry);
            docComparator.CompareDocuments();

            // Mark unmatched docs
            unmatchedDocMarker = new UnmatchedDocMarker(docRepo1CDO.UnmatchedDocs, docRepoRegistry.UnmatchedDocs);
            unmatchedDocMarker.MarkDocuments();

            // Generate reports
            fileWriter = new FileWriterTXT(progParamsRepo.Common.ProgramReportFilePath);
            docAmounts = new DocAmountsRepository(new DocAmounts(docRepo1CDO), new DocAmounts(docRepoRegistry));
            docAmountsReporter = new DocAmountsReporter(docAmounts, docRepoRegistry);
            docAmountsReporter.Notify += consoleWriter.ReportSpecial;
            docAmountsReporter.Notify += fileWriter.ReportSpecial;
            docAmountsReporter.CreateReport();

            // Generate result spreadsheets
            GenerateOutputSpreadsheets();
            CloseProgram();

            void GenerateOutputSpreadsheets()
            {
                spsWriter = new SpreadSheetWriterXLSX(progParamsRepo.Spreadsheets1CDO.Unmatched);
                spsWriter.CreateSpreadsheet(docRepo1CDO.UnmatchedDocs);
                spsWriter = new SpreadSheetWriterXLSX(progParamsRepo.SpreadsheetsRegistry.Unmatched);
                spsWriter.CreateSpreadsheet(docRepoRegistry.UnmatchedDocs, false);

                if (progParamsRepo.Common.IsPrintMatchedDocuments)
                {
                    spsWriter = new SpreadSheetWriterXLSX(progParamsRepo.Spreadsheets1CDO.Matched);
                    spsWriter.CreateSpreadsheet(docRepo1CDO.MatchedDocs, false);
                    spsWriter = new SpreadSheetWriterXLSX(progParamsRepo.SpreadsheetsRegistry.Matched);
                    spsWriter.CreateSpreadsheet(docRepoRegistry.MatchedDocs, false);
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