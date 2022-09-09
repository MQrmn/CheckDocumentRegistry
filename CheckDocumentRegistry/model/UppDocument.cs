
using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    internal class UppDocument
    {
        public string docDate { get; }
        public int docType { get; }
        public string docCompany { get; }
        public string docCounterparty { get; }
        public string docNumber { get; }
        public float docSum { get; }
        public bool isUpd { get; set; }
         
        public UppDocument(string[] docValues)
        {
            this.docDate = docValues[1];
            this.docType = this.GetDocType(docValues[3]);
            this.docCompany = docValues[5];
            this.docCounterparty = docValues[6];
            this.docNumber = docValues[2];
            this.docSum = this.GetDocSum(docValues[7]); 
        }

        int GetDocType(string input)
        {
            return input switch
            {
                "Поступление товаров и услуг" => 1,
                "Счет-фактура полученный" => 2,
                _ => 0

            };
        }

        float GetDocSum(string stringSum)
        {
            string pattern = @"[\.]";
            string regexResult = Regex.Replace(stringSum, pattern, ",", RegexOptions.IgnoreCase);
            return float.Parse(regexResult);
        }


    }
}
