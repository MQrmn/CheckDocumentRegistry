namespace RegComparator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IArrToObjConverter arrToObjConverter;
            IUserReporter userReporter;
            ISpreadSheetReader spreadSheetReader;
            SpreadsheetsPathsBase spreadsheetsPaths1CDO, spreadsheetsPathsRegistry;
            FieldsSettingsRepositoryBase fieldsSettings;
            DocRepositoryBase doc1CDORepository, docRegistryRepository;
            FieldsSettingsRepositoryBase fieldsSettings1CDO, fieldsSettingsRegistry;
            DocRepositoryFiller docRepoFiller1CDO, docRepoFillerRegidtry;


            DocComparator docComparator;
            UnmatchedDocCommentSetter unmatchedDocsCommentator;
            RootConfigFilePath configFilesPath;

            // Get parameters
            RootConfigFilePath rootConfigFilePath = new();
            CommonParams commonParams = new();
            spreadsheetsPaths1CDO = new SpreadsheetsPaths1CDO();
            spreadsheetsPathsRegistry = new SpreadsheetsPathsRegistry();
            commonParams.SetDefaults();
            spreadsheetsPaths1CDO.SetDefaults();
            spreadsheetsPathsRegistry.SetDefaults();

            // Create Instances
            spreadSheetReader = new SpreadSheetReaderXLSX();
            // Create document repositories
            doc1CDORepository = new DocRepository1CDO();
            docRegistryRepository = new DocRepositoryRegistry();
            // Create document fields settings repositories
            fieldsSettings1CDO = new FieldsSettings1CDORepository();
            fieldsSettingsRegistry = new FieldsSettingsRegistryRepository();

            //fieldsSettings = new FieldsSettingsRepository();
            DocAmountReportData reportDocAmount = new();
            //CommonParams workParams = GetWorkParams(args);                    // Getting program parameters
            userReporter = new ConsoleWriter();
            // WorkAbilityChecker.CheckFiles(workParams);                    // Checkimg for existing files to comparing

            ArrToObjConverter srrToObjConverter = new ArrToObjConverter();
            srrToObjConverter.ErrNotify += userReporter.ReportError;

            DocLoader docLoader = new(srrToObjConverter, spreadSheetReader);
            docLoader.Notify += userReporter.ReportInfo;
            // Filling repositories
            docRepoFiller1CDO = new(    docLoader, 
                                        fieldsSettings1CDO, 
                                        doc1CDORepository,
                                        //workParams.inputSpreadsheetDocManagePath,
                                        spreadsheetsPaths1CDO.Source,
                                        //workParams.passSpreadsheetDocManagePath
                                        spreadsheetsPaths1CDO.Skipped
                                        );
            docRepoFillerRegidtry = new(docLoader,
                                        fieldsSettingsRegistry,
                                        docRegistryRepository,
                                        //workParams.inputSpreadsheetDocRegistryPath,
                                        spreadsheetsPathsRegistry.Source,
                                        //workParams.passSpreadSheetDocRegistryPath
                                        spreadsheetsPathsRegistry.Skipped
                                        );

            docRepoFiller1CDO.FillRepository();
            docRepoFillerRegidtry.FillRepository();

            FillSrcDocAmount();
            // Comparing
            CompareDocuments();
            FillResultDocAmount();

            DocumentAmountReporter documentsAmountReporter = new(commonParams.ProgramReportFilePath);
            documentsAmountReporter.CreateAllReports(
                                                    docRegistryRepository.SourceDocs,
                                                    reportDocAmount
                                                    );

            // Result spreadsheets generating
            GenerateOutputSpreadsheets();
            CloseProgram();

            void CompareDocuments()
            {
                docComparator = new(
                                    doc1CDORepository.SourceDocs,
                                    docRegistryRepository.SourceDocs,
                                    docRegistryRepository.SkippedDocs,
                                    docRegistryRepository.SkippedDocs
                                    );
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
                    configFilesPath.CommonParamsFilePath = args[1];
                }
            }

            CommonParams GetWorkParams(string[] args)
            {
                configFilesPath = new();
                ArgsHandler(args);
                WorkParametersReadWrite workParametersReadWrite = new(configFilesPath.CommonParamsFilePath);
                return workParametersReadWrite.GetProgramParameters();
            }

            void GenerateOutputSpreadsheets()
            {
                SpreadSheetWriterXLSX spreadsheetWriterPassedDo;                
                SpreadSheetWriterXLSX spreadSheetWriterPassedUpp;               

                spreadsheetWriterPassedDo = new(spreadsheetsPaths1CDO.Unmatched);
                spreadsheetWriterPassedDo.CreateSpreadsheet(docComparator.UnmatchedDocs1CDO);

                spreadSheetWriterPassedUpp = new(spreadsheetsPathsRegistry.Unmatched);
                spreadSheetWriterPassedUpp.CreateSpreadsheet(docComparator.UnmatchedDocs1CUPP, false);

                if (commonParams.IsPrintMatchedDocuments)
                {
                    SpreadSheetWriterXLSX spreadSheetWriterMatchedDo;               
                    SpreadSheetWriterXLSX spreadSheetWriterMatcheUpp;               

                    spreadSheetWriterMatchedDo = new(spreadsheetsPaths1CDO.Matched);
                    spreadSheetWriterMatchedDo.CreateSpreadsheet(docComparator.MatchedDocs1CDO, false);

                    spreadSheetWriterMatcheUpp = new(spreadsheetsPathsRegistry.Matched);
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
                if (commonParams.IsAskAboutCloseProgram)
                {
                    Console.WriteLine("Для завершения программы нажмите любую клавишу.");
                    Console.ReadKey();
                }
            }
        }
    }
}