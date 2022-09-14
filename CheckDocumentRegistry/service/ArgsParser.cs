using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDocumentRegistry
{
    public class ArgsParser : Arguments
    {
        string doSpreadSheetPathKey = "-do";
        string uppSpreadSheetPathKey = "-upp";
        string matchedDoPathKey = "-mdo";
        string matchedUppPathKey = "-mupp";
        string unMatchedDoPathKey = "-umdo";
        string unMatchedUppPathKey = "-umupp";
        string tmpPath = "100";

        DefaultsRepository defaults = new();

        public ArgsParser(string[] args)
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
