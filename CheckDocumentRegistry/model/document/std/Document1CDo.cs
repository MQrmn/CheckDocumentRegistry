using System;
using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    public class Document1CDo : Document
    {
        private protected Document1CDo() { }

        public Document1CDo(string[] document, int[] docFieldIndex)
        {
            this.Type = this.GetDocType(document[docFieldIndex[0]]);
            this.Title = document[docFieldIndex[1]];
            this.Company = document[docFieldIndex[2]];
            this.Counterparty = this.GetDocCounterparty(document[docFieldIndex[3]]);
            this.Number = this.SetDocNumber(document[docFieldIndex[4]]);
            this.Date = document[docFieldIndex[5]];

            if (document[docFieldIndex[6]] != String.Empty)
                this.Salary = this.GetDocSum(document[docFieldIndex[6]]);
            if (document[7] == "Да") this.IsUpd = true;
        }

        private int GetDocType(string rawDocType)
        {
            return rawDocType switch
            {
                "Приобретение товаров и услуг" => 1,
                "Входящий Акт выполненных работ" => 1,
                "Входящая Счет-Фактура" => 2,
                _ => 0
            };
        }

        private float GetDocSum(string stringSum)
        {
            string pattern = @"[A-Z\s]";
            string regexResult = Regex.Replace(stringSum, pattern, String.Empty, RegexOptions.IgnoreCase);

            return float.Parse(regexResult);
        }

        private protected string GetDocCounterparty(string docCounterparty)
        {
            string pattern = @"\s\([/\s\d]*\)";
            string regexResult = Regex.Replace(docCounterparty, pattern, String.Empty, RegexOptions.IgnoreCase);
            return regexResult;
        }
    }
}
