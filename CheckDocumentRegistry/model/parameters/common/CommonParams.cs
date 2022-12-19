﻿
namespace RegComparator
{
    public class CommonParams
    {
        public string SpreadsheetParams1CDO;
        public string SpreadsheetParamsRegistry;
        public string ProgramReportFilePath;
        public bool IsPrintMatchedDocuments;
        public bool IsAskAboutCloseProgram;
        public string RegistryMode;
        public void SetDefaults()
        {
            SpreadsheetParams1CDO = "SpreadsheetParams1CDO.json";
            SpreadsheetParamsRegistry = "SpreadsheetParamsRegistry.json";
            ProgramReportFilePath = "C:\\1C\\АРХИВНЫЕ ДОКУМЕНТЫ\\РЕЗУЛЬТАТЫ СРАВНЕНИЯ\\report.txt";
            IsPrintMatchedDocuments = false;
            IsAskAboutCloseProgram = true;
            RegistryMode = "KA";
        }
    }
}
