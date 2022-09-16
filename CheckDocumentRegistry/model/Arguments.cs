
namespace CheckDocumentRegistry
{
    public class Arguments
    {
        public string doSpreadSheetPath { get; set; }
        public string uppSpreadSheetPath { get; set; }
        public string doIgnoreSpreadSheetPath { get; set; }
        public string matchedDoPath { get; set; }
        public string matchedUppPath { get; set; }
        public string unMatchedDoPath { get; set; }
        public string unMatchedUppPath { get; set; }
        
        public Arguments() { }

        public Arguments(bool isDefault)
        {
            this.doSpreadSheetPath = "InputDO.xlsx";
            this.uppSpreadSheetPath = "InputUPP.xlsx";
            this.doIgnoreSpreadSheetPath = "InputIgnoreDO.xlsx";
            this.matchedDoPath = "МatchedDO.xlsx";
            this.matchedUppPath = "МatchedUPP.xlsx";
            this.unMatchedDoPath = "UnМatchedDO.xlsx";
            this.unMatchedUppPath = "UnМatchedUpp.xlsx";
        }
    }


}
