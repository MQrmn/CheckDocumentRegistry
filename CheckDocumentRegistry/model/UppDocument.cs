
using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    public class UppDocument : Document
    {

        public UppDocument() {}

        public UppDocument(string[] docValues)
        {
            this.docType = this.GetDocType(docValues[0]);
            this.docTitle = docValues[1];
            this.docDate = docValues[2];
            this.docCounterparty = docValues[3];
            this.docNumber = this.SetDocNumber(docValues[4]);
            this.docCompany = docValues[5];
            
            if (docValues[6] != String.Empty)
                this.docSum = this.GetDocSum(docValues[6]);
        }

        int GetDocType(string input)
        {
            return input switch
            {
                "Да Поступление товаров и услуг" => 1,
                "Да Счет-фактура полученный" => 2,
                _ => 0
            };
        }

        float GetDocSum(string stringSum)
        {
            string pattern = @"[\.]";
            string regexResult = Regex.Replace(stringSum, pattern, ",", RegexOptions.IgnoreCase);
            return float.Parse(regexResult);
        }

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
