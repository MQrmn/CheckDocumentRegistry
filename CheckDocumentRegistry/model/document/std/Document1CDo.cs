using System;
using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    public class Document1CDO : Document
    {

        public Document1CDO(string[] docValues)
        {

            this.Type = this.GetDocType(docValues[0]);
            this.Title = docValues[1];
            this.Company = docValues[2];
            this.Counterparty = this.GetDocCounterparty(docValues[3]);
            this.Number = this.SetDocNumber(docValues[4]);
            this.Date = docValues[5];

            if (docValues[6] != String.Empty)
                this.Salary = this.GetDocSum(docValues[6]);

            if (docValues[7] == "Да") this.IsUpd = true;
        }

        int GetDocType(string rawDocType)
        {
            return rawDocType switch
            {
                "Приобретение товаров и услуг" => 1,
                "Входящий Акт выполненных работ" => 1,
                "Входящая Счет-Фактура" => 2,
                _ => 0
            };
        }

        float GetDocSum(string stringSum)
        {
            string pattern = @"[A-Z\s]";
            string regexResult = Regex.Replace(stringSum, pattern, String.Empty, RegexOptions.IgnoreCase);

            return float.Parse(regexResult);
        }
        
        string GetDocCounterparty(string docCounterparty)
        {
            string pattern = @"\s\([/\s\d]*\)";
            string regexResult = Regex.Replace(docCounterparty, pattern, String.Empty, RegexOptions.IgnoreCase);
            return regexResult;
        }

        

    }
}
