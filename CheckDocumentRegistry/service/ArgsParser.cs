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
        
        public ArgsParser(string[] args)
        { 
            Arguments def = DefaultsRepository.GetDefaults();

            this.doSpreadSheetPath  =   this.SetDoSpreadSheetPath(def.doSpreadSheetPath, args);
            this.uppSpreadSheetPath =   this.SetUppSpreadSheetPath(def.uppSpreadSheetPath, args);
            this.matchedDoPath      =   this.SetMatchedDoPath(def.matchedDoPath, args);
            this.matchedUppPath     =   this.SetMatchedUppPath(def.matchedUppPath, args);
            this.unMatchedDoPath    =   this.SetUnMatchedDoPath(def.unMatchedDoPath, args);
            this.unMatchedUppPath   =   this.SetUnMatchedUppPath(def.unMatchedUppPath, args);

        }

        string SetDoSpreadSheetPath(string def, string[] args)
        {
            return def;
        }

        string SetUppSpreadSheetPath(string def, string[] args)
        {
            return def;
        }

        string SetMatchedDoPath(string def, string[] args)
        {
            return def;
        }

        string SetMatchedUppPath(string def, string[] args)
        {
            return def;
        }

        string SetUnMatchedDoPath(string def, string[] args)
        {
            return def;
        }

        string SetUnMatchedUppPath(string def, string[] args)
        {
            return def;
        }
    }
}
