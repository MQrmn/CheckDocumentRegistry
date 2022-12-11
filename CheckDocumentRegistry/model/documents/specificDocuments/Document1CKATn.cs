using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

namespace RegComparator
{
    public class Document1CKATn : Document
    {

        public Document1CKATn(string[] docFields, int[] docFieldsIndex) : base(docFields, docFieldsIndex) 
        {
        }

        internal protected override int GetDocType(string docName)
        {
            string patternTn = @"Приобретение товаров и услуг";
            string patternSf = @"Счет-фактура";
            bool regexResult = Regex.IsMatch(docName, patternTn, RegexOptions.IgnoreCase);

            rere

            return 1;
        }

        internal protected override float GetDocSalary(string stringSum)
        {
            float floatSum;
            if (stringSum != String.Empty)
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
