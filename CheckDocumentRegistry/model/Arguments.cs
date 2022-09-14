
namespace CheckDocumentRegistry
{
    public class Arguments
    {
        public string doSpreadSheetPath { get; set; }
        public string uppSpreadSheetPath { get; set; }
        public string matchedDoPath { get; set; }
        public string matchedUppPath { get; set; }
        public string unMatchedDoPath { get; set; }
        public string unMatchedUppPath { get; set; }

        public Arguments()
        {

        }

        public Arguments(bool isDefault)
        {
            this.doSpreadSheetPath = "DocumentReportDO.xlsx";
            this.uppSpreadSheetPath = "DocumentReportUPP.xlsx";
            this.matchedDoPath = "МatchedDoDocuments.xlsx";
            this.matchedUppPath = "МatchedUppDocuments.xlsx";
            this.unMatchedDoPath = "UnМatchedDoDocuments.xlsx";
            this.unMatchedUppPath = "UnМatchedUppDocuments.xlsx";
        }

    }


}
