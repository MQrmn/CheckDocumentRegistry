


namespace CheckDocumentRegistry
{
    public class Arguments
    {
        public string doSpreadSheetPath { get; set; }
        public string uppSpreadSheetPath { get; set; }
        public string doIgnoreSpreadSheetPath { get; set; }
        public string matchedDoPath { get; set; }
        public string matchedUppPath { get; set; }
        public string passedDoPath { get; set; }
        public string passedUppPath { get; set; }
        
        public Arguments() { }

        public Arguments(bool isDefault)
        {
            this.doSpreadSheetPath = ".\\input\\DO.xlsx";
            this.uppSpreadSheetPath = ".\\input\\UPP.xlsx";
            this.doIgnoreSpreadSheetPath = ".\\input\\IgnoreDO.xlsx";
            this.matchedDoPath = ".\\output\\MatchedDO.xlsx";
            this.matchedUppPath = ".\\output\\MatchedUPP.xlsx";
            this.passedDoPath = ".\\output\\PassedDO.xlsx";
            this.passedUppPath = ".\\output\\PassedUpp.xlsx";
        }
    }


}
