
using System.Text.RegularExpressions;

namespace RegComparator
{
    public class Document1CKA : Document
    {

        public Document1CKA(string[] docFields, int[] docFieldsIndex) : base(docFields, docFieldsIndex)
        {
        }

        public override int GetDocType(string docName)
        {
            int typeCode = 0;
            string patternTn = @"Приобретение товаров и услуг";
            string patternSf = @"Счет-фактура";

            bool RegexResult(string pattern) => Regex.IsMatch(docName, pattern, RegexOptions.IgnoreCase);

            if (RegexResult(patternTn))
                typeCode = 1;
            else if (RegexResult(patternSf))
                typeCode = 2;
            else
                throw new Exception();
            //Console.WriteLine(typeCode);

            return typeCode;
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
