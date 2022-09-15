
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
            this.doSpreadSheetPath = "DocumentsDO.xlsx";
            this.uppSpreadSheetPath = "DocumentsUPP.xlsx";
            this.matchedDoPath = ".//output//МatchedDoDocuments.xlsx";
            this.matchedUppPath = ".//output//МatchedUppDocuments.xlsx";
            this.unMatchedDoPath = ".//output//UnМatchedDoDocuments.xlsx";
            this.unMatchedUppPath = ".//output//UnМatchedUppDocuments.xlsx";
        }

    }


}
