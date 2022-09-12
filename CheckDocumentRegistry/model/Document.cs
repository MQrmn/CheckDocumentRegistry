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
            string isUpd = this.isUpd ? "Да" : "Нет";

            string[] result = new string[] { 
                                             this.docTitle,
                                             this.docCounterparty,
                                             this.docCompany,
                                             this.docDate,
                                             this.docNumber,
                                             this.docSum.ToString(),
                                             isUpd
            };
            return result;
        }
    }


}
