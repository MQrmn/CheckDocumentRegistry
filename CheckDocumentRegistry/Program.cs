namespace RegComparator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IArrToObjConverter arrToObjConverter;
            IUserReporter userReporter;
            ISpreadSheetReader spreadSheetReader = new SpreadSheetReaderXLSX();
            FieldsSettingsRepositoryBase fieldsSettings;
            DocRepositoryBase doc1CDORepository, docRegistryRepository;
            FieldsSettingsRepositoryBase fieldsSettings1CDO, fieldsSettingsRegistry;
            DocRepositoryFiller docRepoFiller1CDO, docRepoFillerRegidtry;

            DocComparator docComparator;                                    // Class contains results of documents comparing 
            UnmatchedDocCommentSetter unmatchedDocsCommentator;             // Class set comments in unmatched documents
            ConfigFilesPath configFilesPath;

            doc1CDORepository = new DocRepository1CDO();
            docRegistryRepository = new DocRepositoryRegistry();
            fieldsSettings1CDO = new FieldsSettings1CDORepository();
            fieldsSettingsRegistry = new FieldsSettingsRegistryRepository();

            //fieldsSettings = new FieldsSettingsRepository();
            DocAmountReportData reportDocAmount = new();
            WorkParams workParams = GetWorkParams(args);                    // Getting program parameters
            userReporter = new ConsoleWriter();
            // WorkAbilityChecker.CheckFiles(workParams);                    // Checkimg for existing files to comparing

            ArrToObjConverter srrToObjConverter = new ArrToObjConverter();
            srrToObjConverter.ErrNotify += userReporter.ReportError;

            DocLoader docLoader = new(srrToObjConverter, spreadSheetReader);
            docLoader.Notify += userReporter.ReportInfo;
            // Filling 1C:DO repository
            docRepoFiller1CDO = new(    docLoader, 
                                        fieldsSettings1CDO, 
                                        doc1CDORepository,
                                        workParams.inputSpreadsheetDocManagePath,
                                        workParams.passSpreadsheetDocManagePath
                                        );
            docRepoFillerRegidtry = new(docLoader,
                                        fieldsSettingsRegistry,
                                        docRegistryRepository,
                                        workParams.inputSpreadsheetDocRegistryPath,
                                        workParams.passSpreadSheetDocRegistryPath
                                        );

            docRepoFiller1CDO.FillRepository();
            docRepoFillerRegidtry.FillRepository();

            FillSrcDocAmount();
            CompareDocuments();                                             // Comparing
            FillResultDocAmount();

            DocumentAmountReporter documentsAmountReporter = new(workParams.programReportFilePath);
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
                    configFilesPath.ProgramParametersFilePath = args[1];
                }
            }

            WorkParams GetWorkParams(string[] args)
            {
                configFilesPath = new();
                ArgsHandler(args);
                WorkParametersReadWrite workParametersReadWrite = new(configFilesPath.ProgramParametersFilePath);
                return workParametersReadWrite.GetProgramParameters();
            }

            void GenerateOutputSpreadsheets()
            {
                SpreadSheetWriterXLSX spreadsheetWriterPassedDo;                
                SpreadSheetWriterXLSX spreadSheetWriterPassedUpp;               

                spreadsheetWriterPassedDo = new(workParams.outputUnmatchDocManagePath);
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
                if (workParams.isAskAboutCloseProgram)
                {
                    Console.WriteLine("Для завершения программы нажмите любую клавишу.");
                    Console.ReadKey();
                }
            }
        }
    }
}