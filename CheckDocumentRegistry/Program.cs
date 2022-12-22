namespace RegComparator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IUserReporter userReporter;                                             // Reports providing
            IObjsConverter objectConverter;                                         // Objects Reader-Writer in/to file
            ISpreadSheetReader spreadSheetReader;                                   // Getting data from spreadsheetd
            IArrToObjConverter arrToObjConverter;                                   // Getting objs from file, putting objs to file
            FileExistChecker fileExistChecker;

            // Parameters
            RootConfig rootConfigFilePath;                                  // Contains main config file path
            ProgramParamsRepositoryBase progParamsRepo;                             // Contains programs parameters
            SpreadsheetsPathsBase spreadsheetsPaths1CDO, spreadsheetsPathsRegistry; // Contains spreadsheets paths in file system
            FieldsSettingsRepositoryBase fieldsSettings1CDO, fieldsSettingsRegistry;

            // Documents
            DocRepositoryBase doc1CDORepository, docRegistryRepository;
            DocRepositoryFiller docRepoFiller1CDO, docRepoFillerRegidtry;

            // NOT REFACTORED
            UnmatchedDocCommentSetter unmatchedDocsCommentator;
            DocComparator docComparator;

            // CRETING INSTANCES
            userReporter = new ConsoleWriter();
            objectConverter = new ReadWriteJSON();

            // Set program patameters
            ArgsHandler argsHandler = new(args);
            rootConfigFilePath = argsHandler.GetParams();
            progParamsRepo = new ProgramParamsRepository(objectConverter, rootConfigFilePath);

            // Check spreadsheets existing
            fileExistChecker = new();
            fileExistChecker.CheckCritical(progParamsRepo.GetSourcePaths());
            fileExistChecker.CheckNonCritical(progParamsRepo.GetSkippedPaths());

            // Creating documents processors
            spreadSheetReader = new SpreadSheetReaderXLSX();
            arrToObjConverter = new ArrToObjConverter();
            arrToObjConverter.ErrNotify += userReporter.ReportError;
            DocLoader docLoader = new(arrToObjConverter, spreadSheetReader);
            docLoader.Notify += userReporter.ReportInfo;

            // Creating document fields settings repositories
            fieldsSettings1CDO = new FieldsSettings1CDORepository();
            fieldsSettingsRegistry = new FieldsSettingsRegistryRepository();

            // Creating Documents repositories
            doc1CDORepository = new DocRepository1CDO();
            docRegistryRepository = new DocRepositoryRegistry();

            DocAmountReportData reportDocAmount = new();

            // Creating documents repositories fillers
            docRepoFiller1CDO     = new(docLoader, 
                                        fieldsSettings1CDO, 
                                        doc1CDORepository,
                                        progParamsRepo.Spreadsheets1CDO
                                        );
            docRepoFillerRegidtry = new(docLoader,
                                        fieldsSettingsRegistry,
                                        docRegistryRepository,
                                        progParamsRepo.SpreadsheetsRegistry
                                        );
            // Filling doc repositories 
            docRepoFiller1CDO.FillRepository();
            docRepoFillerRegidtry.FillRepository();

            FillSrcDocAmount();
            // Comparing documents
            CompareDocuments();
            FillResultDocAmount();
            
            DocumentAmountReporter documentsAmountReporter = new(progParamsRepo.Common.ProgramReportFilePath);
            documentsAmountReporter.CreateAllReports(
                                                    docRegistryRepository.SourceDocs,
                                                    reportDocAmount
                                                    );
            // Result spreadsheets generating
            GenerateOutputSpreadsheets();
            CloseProgram();

            void CompareDocuments()
            {
                docComparator = new(doc1CDORepository.SourceDocs,
                                    docRegistryRepository.SourceDocs,
                                    docRegistryRepository.SkippedDocs,
                                    docRegistryRepository.SkippedDocs
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
                reportDocAmount.doDocumentsCount = doc1CDORepository.SourceDocs.Count;
                reportDocAmount.uppDocumentsCount = docRegistryRepository.SourceDocs.Count;
                reportDocAmount.ignoreDoDocumentsCount = doc1CDORepository.SkippedDocs.Count;
                reportDocAmount.ignoreUppDocumentsCount = docRegistryRepository.SkippedDocs.Count;
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