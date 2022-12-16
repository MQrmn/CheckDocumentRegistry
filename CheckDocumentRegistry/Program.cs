﻿namespace RegComparator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DocComparator docComparator;                                    // Class contains results of documents comparing 
            UnmatchedDocCommentSetter unmatchedDocsCommentator;             // Class set comments in unmatched documents
            ConfigFilesPath configFilesPath;

            DocFieldsSettingsRepository docFieldsSettingsRepository = new();
            DocRepository docRepository = new();
            
            DocLoader _docLoader;
            IArrToObjConverter arrToObjConverter;
            DocAmountReportData reportDocAmount = new();
            WorkParams workParams = GetWorkParams(args);                    // Getting program parameters
           // WorkAbilityChecker.CheckFiles(workParams);                    // Checkimg for existing files to comparing

            GetSrcDocs1CDO();                                               // Getting documents from 1C:DO
            GetRegistryDocs();                                              // Getting documents from 1C:RF or UPP registry
            GetIgnoreDocList();                                             // Getting ignored documents from spreadsheets
            FillSrcDocAmount();
            CompareDocuments();                                             // Comparing
            FillResultDocAmount();

            DocumentAmountReporter documentsAmountReporter = new(workParams.programReportFilePath);
            documentsAmountReporter.CreateAllReports(docRepository.SrcRegistry, reportDocAmount);

            // Result spreadsheets generating
            GenerateOutputSpreadsheets();
            CloseProgram();

            void GetSrcDocs1CDO()
            {
                arrToObjConverter = new ArrToObjConverter<Document1CDO>(docFieldsSettingsRepository.DocFieldsDO,
                                                          docRepository.Src1CDO);

                _docLoader = new(arrToObjConverter);

                _docLoader.GetDocObjectList<Document1CDO>(workParams.inputSpreadsheetDocManagePath, workParams.exceptedDocManagePath);
            }

            void GetRegistryDocs(){
                if (workParams.programMode == "UPP")
                    GetSrcDocs1CUPP();
                else
                    GetSrcDocs1CKA();
            }

            void GetSrcDocs1CUPP()
            {
                _docLoader = new(docFieldsSettingsRepository.DocFieldsRegUPP, docRepository.SrcRegistry);
                _docLoader.GetDocObjectList<Document1CUPP>(workParams.inputSpreadsheetDocRegistryPath, 
                                                           workParams.exceptedDocRegistryPath);
            }

            void GetSrcDocs1CKA()
            {
                _docLoader = new(docFieldsSettingsRepository.DocFieldsKA, docRepository.SrcRegistry);
                _docLoader.GetDocObjectList<Document1CKA>(workParams.inputSpreadsheetDocRegistryPath, 
                                                          workParams.exceptedDocRegistryPath);
            }

            void GetIgnoreDocList()
            {
                _docLoader = new(docFieldsSettingsRepository.DocFieldsCmn, docRepository.Pass1CDO);
                _docLoader.GetDocObjectList<Document>(workParams.passSpreadsheetDocManagePath);

                _docLoader = new(docFieldsSettingsRepository.DocFieldsCmn, docRepository.PassRegistry);
                _docLoader.GetDocObjectList<Document>(workParams.passSpreadSheetDocRegistryPath);

                //docRepository.Pass1CDO = docLoader.GetDocsPass(workParams.passSpreadsheetDocManagePath);
                //docRepository.PassRegistry = docLoader.GetDocsPass(workParams.passSpreadSheetDocRegistryPath);
            }

            void CompareDocuments()
            {
                docComparator = new(docRepository.Src1CDO, docRepository.SrcRegistry, 
                                    docRepository.Pass1CDO, docRepository.PassRegistry);
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
                reportDocAmount.doDocumentsCount = docRepository.Src1CDO.Count;
                reportDocAmount.uppDocumentsCount = docRepository.SrcRegistry.Count;
                reportDocAmount.ignoreDoDocumentsCount = docRepository.Pass1CDO.Count;
                reportDocAmount.ignoreUppDocumentsCount = docRepository.PassRegistry.Count;
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