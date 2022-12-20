namespace RegComparator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Reporter
            IUserReporter userReporter = new ConsoleWriter();
            // Objects Reader-Writer in/to file
            IObjsSerialiser objectSerialiser = new ReadWriteJSON();

            // Program parameters
            RootConfigFilePath rootConfigFilePath = new();

            ProgramParamsRepositoryBase programParamsRepository = 
                new ProgramParamsRepository(objectSerialiser, rootConfigFilePath);

            ISpreadSheetReader spreadSheetReader = new SpreadSheetReaderXLSX();
            IArrToObjConverter arrToObjConverter = new ArrToObjConverter();
            arrToObjConverter.ErrNotify += userReporter.ReportError;
            
            SpreadsheetsPathsBase spreadsheetsPaths1CDO, spreadsheetsPathsRegistry;
            FieldsSettingsRepositoryBase fieldsSettings;
            DocComparator docComparator;
            UnmatchedDocCommentSetter unmatchedDocsCommentator;
            RootConfigFilePath configFilesPath;

            // Documents repository, DocSettings repository, repository filler
            DocRepositoryBase doc1CDORepository, docRegistryRepository;
            FieldsSettingsRepositoryBase fieldsSettings1CDO, fieldsSettingsRegistry;
            DocRepositoryFiller docRepoFiller1CDO, docRepoFillerRegidtry;

            // Documents repository
            doc1CDORepository = new DocRepository1CDO();
            docRegistryRepository = new DocRepositoryRegistry();
            // Create document fields settings repositories
            fieldsSettings1CDO = new FieldsSettings1CDORepository();
            fieldsSettingsRegistry = new FieldsSettingsRegistryRepository();

            DocAmountReportData reportDocAmount = new();
            //userReporter = new ConsoleWriter();
            // WorkAbilityChecker.CheckFiles(workParams);                    // Checkimg for existing files to comparing

            //arrToObjConverter = new ArrToObjConverter();
            

            DocLoader docLoader = new(arrToObjConverter, spreadSheetReader);

            docLoader.Notify += userReporter.ReportInfo;


            // Filling repositories
            docRepoFiller1CDO = new(    docLoader, 
                                        fieldsSettings1CDO, 
                                        doc1CDORepository,
                                        programParamsRepository.Spreadsheets1CDO
                                        //spreadsheetsPaths1CDO
                                        );
            docRepoFillerRegidtry = new(docLoader,
                                        fieldsSettingsRegistry,
                                        docRegistryRepository,
                                        programParamsRepository.SpreadsheetsRegistry
                                        //spreadsheetsPathsRegistry
                                        );

            docRepoFiller1CDO.FillRepository();
            docRepoFillerRegidtry.FillRepository();

            FillSrcDocAmount();
            // Comparing
            CompareDocuments();
            FillResultDocAmount();
            
            DocumentAmountReporter documentsAmountReporter = new(programParamsRepository.Common.ProgramReportFilePath);
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

            //CommonParams GetWorkParams(string[] args)
            //{
            //    configFilesPath = new();
            //    ArgsHandler(args);
            //    WorkParametersReadWrite workParametersReadWrite = new(configFilesPath.CommonParamsFilePath);
            //    return workParametersReadWrite.GetProgramParameters();
            //}

            void GenerateOutputSpreadsheets()
            {
                SpreadSheetWriterXLSX spreadsheetWriterPassedDo;                
                SpreadSheetWriterXLSX spreadSheetWriterPassedUpp;               

                spreadsheetWriterPassedDo = new(programParamsRepository.Spreadsheets1CDO.Unmatched);
                spreadsheetWriterPassedDo.CreateSpreadsheet(docComparator.UnmatchedDocs1CDO);

                spreadSheetWriterPassedUpp = new(programParamsRepository.SpreadsheetsRegistry.Unmatched);
                spreadSheetWriterPassedUpp.CreateSpreadsheet(docComparator.UnmatchedDocs1CUPP, false);

                if (programParamsRepository.Common.IsPrintMatchedDocuments)
                {
                    SpreadSheetWriterXLSX spreadSheetWriterMatchedDo;               
                    SpreadSheetWriterXLSX spreadSheetWriterMatcheUpp;               

                    spreadSheetWriterMatchedDo = new(programParamsRepository.Spreadsheets1CDO.Matched);
                    spreadSheetWriterMatchedDo.CreateSpreadsheet(docComparator.MatchedDocs1CDO, false);

                    spreadSheetWriterMatcheUpp = new(programParamsRepository.SpreadsheetsRegistry.Matched);
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
                if (programParamsRepository.Common.IsAskAboutCloseProgram)
                {
                    Console.WriteLine("Для завершения программы нажмите любую клавишу.");
                    Console.ReadKey();
                }
            }
        }
    }
}