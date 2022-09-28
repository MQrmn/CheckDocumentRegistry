
using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    public class Document1CUpp : Document
    {

        public Document1CUpp() {}

        public Document1CUpp(string[] docValues)
        {
            this.Type = this.GetDocType(docValues[0]);
            this.Title = docValues[1];
            this.Date = docValues[2];
            this.Counterparty = docValues[3];
            this.Number = this.SetDocNumber(docValues[4]);
            this.Company = docValues[5];
            
            if (docValues[6] != String.Empty)
                this.Salary = this.GetDocSum(docValues[6]);
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
            string[] result = new string[] { this.Type.ToString(),
                                             this.Title,
                                             this.Date,
                                             this.Counterparty,
                                             this.Number,
                                             this.Company,
                                             this.Salary.ToString()
            };
            return result;
        }

    }
}
