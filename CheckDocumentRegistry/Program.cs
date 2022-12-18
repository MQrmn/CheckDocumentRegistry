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


            DocLoader docLoader;
            DocComparator docComparator;                                    // Class contains results of documents comparing 
            UnmatchedDocCommentSetter unmatchedDocsCommentator;             // Class set comments in unmatched documents
            ConfigFilesPath configFilesPath;

            doc1CDORepository = new DocRepository1CDO();
            docRegistryRepository = new DocRepositoryRegistry();
            fieldsSettings1CDO = new FieldsSettings1CDORepository();
            fieldsSettingsRegistry = new FieldsSettingsRegistryRepository();

            fieldsSettings = new FieldsSettingsRepository();
            //DocRepository docRepository = new();
            DocAmountReportData reportDocAmount = new();
            WorkParams workParams = GetWorkParams(args);                    // Getting program parameters
            userReporter = new ConsoleWriter();
            // WorkAbilityChecker.CheckFiles(workParams);                    // Checkimg for existing files to comparing
            
            // Getting documents from 1C:DO
            GetSrcDocs<Document1CDO>(fieldsSettings.FieldsDO,
                                        doc1CDORepository.SourceDocs,
                                        //docRepository.Src1CDO,
                                        workParams.inputSpreadsheetDocManagePath,
                                        workParams.exceptedDocManagePath);
            // Getting documents from 1C:KA or UPP registry
            GetRegistrySrcDocuments();

            // Getting ignored 1C:DO documents 
            GetSrcDocs<Document>(fieldsSettings.FieldsCmn,
                                    doc1CDORepository.SkippedDocs,
                                    //docRepository.Pass1CDO,
                                    workParams.passSpreadsheetDocManagePath);

            // Getting ignored 1C:KA or 1C:UPP documents 
            GetSrcDocs<Document>(fieldsSettings.FieldsCmn,
                                    docRegistryRepository.SkippedDocs,
                                    //docRepository.PassRegistry,
                                    workParams.passSpreadSheetDocRegistryPath);

            //GetIgnoreDocList();                                             
            FillSrcDocAmount();
            CompareDocuments();                                             // Comparing
            FillResultDocAmount();

            DocumentAmountReporter documentsAmountReporter = new(workParams.programReportFilePath);
            documentsAmountReporter.CreateAllReports(
                                                    //docRepository.SrcRegistry, 
                                                    docRegistryRepository.SourceDocs,
                                                    reportDocAmount
                                                    );

            // Result spreadsheets generating
            GenerateOutputSpreadsheets();
            CloseProgram();

            void GetRegistrySrcDocuments(){
                if (workParams.programMode == "KA")
                    GetSrcDocs<Document1CKA>(fieldsSettings.FieldsRegistry,
                                                docRegistryRepository.SourceDocs,
                                                //docRepository.SrcRegistry,
                                                workParams.inputSpreadsheetDocRegistryPath, 
                                                workParams.exceptedDocRegistryPath);
                else
                    GetSrcDocs<Document1CUPP>(fieldsSettings.FieldsRegistry,
                                                docRegistryRepository.SourceDocs,
                                                //docRepository.SrcRegistry,
                                                workParams.inputSpreadsheetDocRegistryPath, 
                                                workParams.exceptedDocRegistryPath);
            }

            // Getting documents from spreadsheets
            void GetSrcDocs<T>(DocFieldsBase fieldsSettings, List<Document> documents, 
                               string[] spreadSheetsPath, string? exceptedDocsPath = null) 
                               where T : Document
            {
                arrToObjConverter = new ArrToObjConverter<T>(fieldsSettings, documents);
                arrToObjConverter.ErrNotify += userReporter.ReportError;

                docLoader = new(arrToObjConverter, spreadSheetReader);
                docLoader.Notify += userReporter.ReportInfo;
                docLoader.GetDocObjectList<T>(spreadSheetsPath, exceptedDocsPath);
            } 

            void CompareDocuments()
            {
                docComparator = new(
                                    //docRepository.Src1CDO,
                                    doc1CDORepository.SourceDocs,
                                    //docRepository.SrcRegistry, 
                                    docRegistryRepository.SourceDocs,
                                    //docRepository.Pass1CDO,
                                    docRegistryRepository.SkippedDocs,
                                    //docRepository.PassRegistry
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
                //reportDocAmount.doDocumentsCount = docRepository.Src1CDO.Count;
                reportDocAmount.doDocumentsCount = doc1CDORepository.SourceDocs.Count;
                //reportDocAmount.uppDocumentsCount = docRepository.SrcRegistry.Count;
                reportDocAmount.uppDocumentsCount = docRegistryRepository.SourceDocs.Count;
                //reportDocAmount.ignoreDoDocumentsCount = docRepository.Pass1CDO.Count;
                reportDocAmount.ignoreDoDocumentsCount = doc1CDORepository.SkippedDocs.Count;
                //reportDocAmount.ignoreUppDocumentsCount = docRepository.PassRegistry.Count;
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