
namespace CheckDocumentRegistry
{
    public class ChangeableParameters
    {
        public string doSpreadSheetPath { get; set; }
        public string uppSpreadSheetPath { get; set; }
        public string doIgnoreSpreadSheetPath { get; set; }
        public string uppIgnoreSpreadSheetPath { get; set; }
        public string matchedDoPath { get; set; }
        public string matchedUppPath { get; set; }
        public string passedDoPath { get; set; }
        public string passedUppPath { get; set; }
        public string exceptedDoPath { get; set; }
        public string exceptedUppPath { get; set; }
        public string reportFilePath { get; set; }
        public bool printMatchedDocuments { get; set; }
        public bool askAboutCloseProgram { get; set; }
        
        

        public void SetDefaults()
        {
            this.doSpreadSheetPath = ".\\input\\DO.xlsx";
            this.uppSpreadSheetPath = ".\\input\\UPP.xlsx";
            this.doIgnoreSpreadSheetPath = ".\\input\\IgnoreDO.xlsx";
            this.uppIgnoreSpreadSheetPath = ".\\input\\IgnoreUPP.xlsx";
            this.matchedDoPath = ".\\output\\MatchedDO.xlsx";
            this.matchedUppPath = ".\\output\\MatchedUPP.xlsx";
            this.passedDoPath = ".\\output\\PassedDO.xlsx";
            this.passedUppPath = ".\\output\\PassedUpp.xlsx";
            this.exceptedDoPath = ".\\output\\exceptedDo.xlsx";
            this.exceptedUppPath = ".\\output\\exceptedUpp.xlsx";
            this.reportFilePath = ".\\output\\report.txt";
            this.printMatchedDocuments = false;
            this.askAboutCloseProgram = true;

        }
    }
}
