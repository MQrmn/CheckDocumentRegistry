
namespace RegComparator
{
    public class UserParameters
    {
        public string inputSpreadsheetDocManagePath { get; set; }
        public string[] inputSpreadsheetDocRegistryPath { get; set; }
        public string passSpreadsheetDocManagePath { get; set; }
        public string passSpreadSheetDocRegistryPath { get; set; }
        public string outputMatchedDocManagePath { get; set; }
        public string outputMatchedDocRestryPath { get; set; }
        public string outputUnmatchDocManagePath { get; set; }
        public string outputUnmatchedDocRegistryPath { get; set; }
        public string exceptedDocManagePath { get; set; }
        public string exceptedDocRegistryPath { get; set; }
        public string programReportFilePath { get; set; }
        public bool isPrintMatchedDocuments { get; set; }
        public bool isAskAboutCloseProgram { get; set; }
        public string programMode { get; set; }


        public void SetDefaults()
        {
            this.inputSpreadsheetDocManagePath = ".\\input\\DocManagement.xlsx";
            this.inputSpreadsheetDocRegistryPath = new string[] { ".\\input\\DocRegistry.xlsx" };
            this.passSpreadsheetDocManagePath = ".\\input\\PassDocManagement.xlsx";
            this.passSpreadSheetDocRegistryPath = ".\\input\\PassDocRegistry.xlsx";
            this.outputMatchedDocManagePath = ".\\output\\MatchedDocManagement.xlsx";
            this.outputMatchedDocRestryPath = ".\\output\\MatchedDocRegistry.xlsx";
            this.outputUnmatchDocManagePath = ".\\output\\UnmatchedDocManagement.xlsx";
            this.outputUnmatchedDocRegistryPath = ".\\output\\UnmatchedDocRegistry.xlsx";
            this.exceptedDocManagePath = ".\\output\\exceptedDocManagement.xlsx";
            this.exceptedDocRegistryPath = ".\\output\\exceptedDocRegistry.xlsx";
            this.programReportFilePath = ".\\output\\report.txt";
            this.isPrintMatchedDocuments = false;
            this.isAskAboutCloseProgram = true;
            this.programMode = "UPP";
        }
    }
}
