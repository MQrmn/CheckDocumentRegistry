
using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    public class Document1CUppStandard : Document
    {

        public Document1CUppStandard() {}

        public Document1CUppStandard(string[] docValues)
        {
            this.Type = this.GetDocType(docValues[0]);
            this.Title = docValues[1];
            this.Date = docValues[3];
            this.Counterparty = docValues[4];
            this.Number = this.SetDocNumber(docValues[5]);
            this.Company = docValues[6];
            
            if (docValues[6] != String.Empty)
                this.Salary = this.GetDocSum(docValues[7]);
        }

        private protected int GetDocType(string input)
        {
            return input switch
            {
                "Да Поступление товаров и услуг" => 1,
                "Да Счет-фактура полученный" => 2,
                _ => 0
            };
        }

        private protected float GetDocSum(string stringSum)
        {
            string pattern = @"[\.]";
            string regexResult = Regex.Replace(stringSum, pattern, ",", RegexOptions.IgnoreCase);
            try
            {
                return float.Parse(regexResult);
            }
            catch
            {
                return 0;
            }
        }


    }
}
