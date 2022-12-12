using System.Text.RegularExpressions;

namespace RegComparator
{ 
    public class Document1CUPP : Document
    {

        public Document1CUPP(string[] docFields, int[] docFieldsIndex) : base(docFields, docFieldsIndex)
        {
        }

        public override int GetDocType(string input)
        {
            return input switch
            {
                "Да Поступление товаров и услуг" => 1,
                "Да Счет-фактура полученный" => 2,
                _ => 0
            };
        }

        public override float GetDocSalary(string stringSum)
        {
            float floatSum;
            if (stringSum != string.Empty)
            {
                string pattern = @"[\.]";
                string regexResult = Regex.Replace(stringSum, pattern, ",", RegexOptions.IgnoreCase);
                floatSum = float.Parse(regexResult);
            }
            else
                floatSum = 0;

            return floatSum;
        }

    }
}
