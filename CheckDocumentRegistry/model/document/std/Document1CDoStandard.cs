using System;
using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    public class Document1CDoStandard : Document
    {
        public Document1CDoStandard() { }
        public Document1CDoStandard(string[] docValues)
        {

            this.Type = this.GetDocType(docValues[0]);
            this.Title = docValues[3];
            this.Company = docValues[6];
            this.Counterparty = this.GetDocCounterparty(docValues[7]);
            this.Number = this.SetDocNumber(docValues[8]);
            this.Date = docValues[9];

            if (docValues[6] != String.Empty)
                this.Salary = this.GetDocSum(docValues[10]);

            if (docValues[11] == "Да") this.IsUpd = true;
        }

        private protected int GetDocType(string rawDocType)
        {
            return rawDocType switch
            {
                "Приобретение товаров и услуг" => 1,
                "Входящий Акт выполненных работ" => 1,
                "Входящая Счет-Фактура" => 2,
                _ => 0
            };
        }

        private protected float GetDocSum(string stringSum)
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
