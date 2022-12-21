﻿
using System.Text.Json.Serialization;

namespace RegComparator
{
    public class CommonParams : IParameters
    {
        public string SpreadsheetParams1CDO;
        public string SpreadsheetParamsRegistry;
        public string ProgramReportFilePath;
        public bool IsPrintMatchedDocuments;
        public bool IsAskAboutCloseProgram;
        public string RegistryMode;
        public void SetDefaults()
        {
            Console.WriteLine(this.GetType());
            SpreadsheetParams1CDO = "SpreadsheetParams1CDO.json";
            SpreadsheetParamsRegistry = "SpreadsheetParamsRegistry.json";
            ProgramReportFilePath = "C:\\1C\\АРХИВНЫЕ ДОКУМЕНТЫ\\РЕЗУЛЬТАТЫ СРАВНЕНИЯ\\report.txt";
            IsPrintMatchedDocuments = false;
            IsAskAboutCloseProgram = true;
            RegistryMode = "KA";
        }

        public void VerifyFields()
        {
            if (SpreadsheetParams1CDO == string.Empty || SpreadsheetParams1CDO is null)
                throw new Exception();
            if (SpreadsheetParamsRegistry == string.Empty || SpreadsheetParamsRegistry is null)
                throw new Exception();
            if (ProgramReportFilePath == string.Empty || ProgramReportFilePath is null)
                throw new Exception();
            if (RegistryMode == string.Empty || RegistryMode is null)
                throw new Exception();
        }
    }
}