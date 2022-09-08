using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDocumentRegistry
{
    public class ArgsParser
    {

        string uppKey = "-upp";
        string doKey = "-do";
        string outputKey = "-o";


        public string doSpreadSheet;
        public string uppSpreadSheet;
        public string outputFileName;
        public ArgsParser(string[] input)
        {
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i] == uppKey)
                    this.uppSpreadSheet = input[i + 1];
                if (input[i] == doKey)
                    this.doSpreadSheet = input[i + 1];
                if (input[i] == outputKey)
                    this.outputFileName = input[i + 1];
            }       

        }
    }
}
