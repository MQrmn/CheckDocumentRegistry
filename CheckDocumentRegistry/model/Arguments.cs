
namespace CheckDocumentRegistry
{
    public class Arguments
    {
        readonly string doSpreadSheetPathKey = "-do";
        readonly string uppSpreadSheetPathKey = "-upp";
        readonly string matchedDoPathKey = "-mdo";
        readonly string matchedUppPathKey = "-mupp";
        readonly string unMatchedDoPathKey = "-umdo";
        readonly string unMatchedUppPathKey = "-umupp";

        public string doSpreadSheetPath { get; }
        public string uppSpreadSheetPath { get; }
        public string matchedDoPath { get; }
        public string matchedUppPath { get; }
        public string unMatchedDoPath { get; }
        public string unMatchedUppPath { get; }

        string tmpPath = "100";

        public Arguments(string[] args)
        {
            this.doSpreadSheetPath = this.SetDoSpreadSheetPath(args);
            this.uppSpreadSheetPath = this.SetUppSpreadSheetPath(args);
            this.matchedDoPath = this.SetMatchedDoPath(args);
            this.matchedUppPath = this.SetMatchedUppPath(args);
            this.unMatchedDoPath = this.SetUnMatchedDoPath(args);
            this.unMatchedUppPath = this.SetUnMatchedUppPath(args);
        }

        string SetDoSpreadSheetPath(string[] args)
        {
            return @$"C:\1C\{tmpPath}\DocumentReportDO.xlsx";
        }

        string SetUppSpreadSheetPath(string[] args)
        {
            return $"C:\\1C\\{tmpPath}\\DocumentReportUPP.xlsx";
        }

        string SetMatchedDoPath(string[] args)
        {
            return "C:\\1C\\МatchedDoDocuments.xlsx";
        }

        string SetMatchedUppPath(string[] args)
        {
            return "C:\\1C\\МatchedUppDocuments.xlsx";
        }

        string SetUnMatchedDoPath(string[] args)
        {
            return "C:\\1C\\UnМatchedDoDocuments.xlsx";
        }

        string SetUnMatchedUppPath(string[] args)
        {
            return "C:\\1C\\UnМatchedUppDocuments.xlsx";
        }


    }


}
