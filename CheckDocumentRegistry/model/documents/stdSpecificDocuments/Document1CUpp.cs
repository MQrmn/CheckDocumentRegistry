
using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    public class Document1CUPP : Document
    {

        public Document1CUPP(string[] document, int[] docFieldIndex)
        {
            this.Type = this.GetDocType(document[docFieldIndex[0]]);
            this.Title = document[docFieldIndex[1]];
            this.Counterparty = this.GetDocCounterparty(document[docFieldIndex[3]]);
            this.Company = document[docFieldIndex[5]];
            this.Date = document[docFieldIndex[2]];
            this.Number = this.GetDocNumber(document[docFieldIndex[4]]);

            if (document[docFieldIndex[6]] != String.Empty)
                this.Salary = this.GetDocSum(document[docFieldIndex[6]]);
        }

        private int GetDocType(string input)
        {
            return input switch
            {
                "Да Поступление товаров и услуг" => 1,
                "Да Счет-фактура полученный" => 2,
                _ => 0
            };
        }

        private float GetDocSum(string stringSum)
        {
            string pattern = @"[\.]";
            string regexResult = Regex.Replace(stringSum, pattern, ",", RegexOptions.IgnoreCase);
            return float.Parse(regexResult);
            
        }

        private string GetDocCounterparty(string counterparty) => counterparty;


    }
}
