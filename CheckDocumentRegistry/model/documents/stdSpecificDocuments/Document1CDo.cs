using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    public class Document1CDO : Document
    {

        public Document1CDO(string[] document, int[] docFieldIndex)
        {
            this.Type = this.GetDocType(document[docFieldIndex[0]]);
            this.Title = document[docFieldIndex[1]];
            this.Counterparty = this.GetCounterParty(document[docFieldIndex[3]]);
            this.Company = document[docFieldIndex[2]];
            this.Date = document[docFieldIndex[5]];
            this.Number = this.GetDocNumber(document[docFieldIndex[4]]);

            if (document[docFieldIndex[6]] != String.Empty)
                this.Salary = this.GetDocSalary(document[docFieldIndex[6]]);

            if (document[docFieldIndex[7]] == "Да") this.IsUpd = true;
        }


        private protected override int GetDocType(string rawDocType)
        {
            return rawDocType switch
            {
                "Приобретение товаров и услуг" => 1,
                "Входящий Акт выполненных работ" => 1,
                "Входящая Счет-Фактура" => 2,
                _ => 0
            };
        }


        private protected override string GetCounterParty(string docCounterparty)
        {
            string pattern = @"\s\([/\s\d]*\)";
            string regexResult = Regex.Replace(docCounterparty, pattern, String.Empty, RegexOptions.IgnoreCase);
            return regexResult;
        }


        private protected override float GetDocSalary(string stringSum)
        {
            string pattern = @"[A-Z\s]";
            string regexResult = Regex.Replace(stringSum, pattern, String.Empty, RegexOptions.IgnoreCase);

            return float.Parse(regexResult);
        }


    }
}
