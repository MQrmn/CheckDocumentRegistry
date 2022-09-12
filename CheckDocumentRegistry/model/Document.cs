using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDocumentRegistry
{
    public class Document
    {
        public int docType { get; set; }
        public string docTitle { get; set; }
        public string docCompany { get; set; }
        public string docCounterparty { get; set; }
        public string docNumber { get; set; }
        public string docDate { get; set; }
        public float docSum { get; set; }
        public bool isUpd { get; set; }

        public string[] GetArray()
        {
            string[] result = new string[] { this.docType.ToString(),
                                             this.docTitle,
                                             this.docDate,
                                             this.docCounterparty,
                                             this.docNumber,
                                             this.docCompany,
                                             this.docSum.ToString()
            };
            return result;
        }
    }


}
