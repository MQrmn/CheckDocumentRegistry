using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    public class Document1CDO : Document
    {

        public Document1CDO(string[] docFields, int[] docFieldsIndex) : base(docFields, docFieldsIndex)
        {
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


        private protected override string GetDocCounterparty(string docCounterparty)
        {
            string pattern = @"\s\([/\s\d]*\)";
            string regexResult = Regex.Replace(docCounterparty, pattern, String.Empty, RegexOptions.IgnoreCase);
            return regexResult;
        }


        private protected override float GetDocSalary(string stringSum)
        {
            float floatSum;
            if (stringSum != String.Empty)
            {
                string pattern = @"[A-Z\s]";
                string regexResult = Regex.Replace(stringSum, pattern, String.Empty, RegexOptions.IgnoreCase);
                floatSum = float.Parse(regexResult); 
            }
            else
                floatSum = 0;

            return floatSum;
        }


    }
}
