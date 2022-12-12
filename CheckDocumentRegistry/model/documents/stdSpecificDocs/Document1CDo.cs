using System.Text.RegularExpressions;

namespace RegComparator
{
    public class Document1CDO : Document
    {

        public Document1CDO(string[] docFields, int[] docFieldsIndex) : base(docFields, docFieldsIndex)
        {
        }

        public override int GetDocType(string rawDocType)
        {
            int docType = rawDocType switch
            {
                "Приобретение товаров и услуг" => 1,
                "Входящий Акт выполненных работ" => 1,
                "Входящая Счет-Фактура" => 2,
                _ => 0
            };

            if (docType == 0) throw new Exception();

            return docType;
        }


        public override string GetDocCounterparty(string docCounterparty)
        {
            string pattern = @"\s\([/\s\d]*\)";
            string regexResult = Regex.Replace(docCounterparty, pattern, string.Empty, RegexOptions.IgnoreCase);
            return regexResult;
        }


        public override float GetDocSalary(string stringSum)
        {
            float floatSum;
            if (stringSum != string.Empty)
            {
                string pattern = @"[A-Z\s]";
                string regexResult = Regex.Replace(stringSum, pattern, string.Empty, RegexOptions.IgnoreCase);
                floatSum = float.Parse(regexResult);
            }
            else
                floatSum = 0;

            return floatSum;
        }


    }
}
