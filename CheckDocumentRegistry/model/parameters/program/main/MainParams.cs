
using System.Text.Json.Serialization;

namespace RegComparator
{
    public class MainParams : ProgramParametersBase
    {
        public string SpreadsheetParams1CDO;
        public string SpreadsheetParamsRegistry;
        public string ProgramReportFilePath;
        public bool IsPrintMatchedDocuments;
        public bool IsAskAboutCloseProgram;
        public string RegistryMode;
        public override void SetDefaults()
        {
            Console.WriteLine(this.GetType());
            SpreadsheetParams1CDO = "Spreadsheets1CDO.json";
            SpreadsheetParamsRegistry = "SpreadsheetsRegistry.json";
            ProgramReportFilePath = "Report.txt";
            IsPrintMatchedDocuments = false;
            IsAskAboutCloseProgram = true;
            RegistryMode = "KA";
        }

        public override void VerifyFields()
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
