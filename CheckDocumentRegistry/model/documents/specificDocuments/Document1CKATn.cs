using System.Text.RegularExpressions;
using RegistryComparator.model.documents.commonDocument;

namespace RegComparator
{
    public class Document1CKATn : Document
    {

        public Document1CKATn(string[] docFields, int[] docFieldsIndex) : base(docFields, docFieldsIndex) 
        {
        }

        internal protected override int GetDocType(string input)
        {
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
