using System.Text.RegularExpressions;

namespace RegistryComparator
{
    public class Document
    {
        internal protected int Type { get; set; }
        internal protected string Title { get; set; }
        internal protected string Company { get; set; }
        internal protected string Counterparty { get; set; }
        internal protected string Number { get; set; }
        internal protected string Date { get; set; }
        internal protected float Salary { get; set; }
        internal protected bool IsUpd { get; set; }
        internal protected string? Comment { get; set; }
        internal protected int StylePosition { get; set; }

        public Document()
        {
        }

        public Document(string[] docFields, int[] docFieldsIndex)
        {
            Type = GetDocType(docFields[docFieldsIndex[0]]);
            Title = docFields[docFieldsIndex[1]];
            Counterparty = GetDocCounterparty(docFields[docFieldsIndex[2]]);
            Company = docFields[docFieldsIndex[3]];
            Date = docFields[docFieldsIndex[4]];
            Number = GetDocNumber(docFields[docFieldsIndex[5]]);
            Salary = GetDocSalary(docFields[docFieldsIndex[6]]);

            if (docFields[docFieldsIndex[docFieldsIndex.Length - 1]] == "Да") IsUpd = true;
            Comment = string.Empty;
        }

        internal protected Document(string[] docFields)
        {}

        internal protected string[] GetArray()
        {
            string isUpd = IsUpd ? "Да" : "Нет";

            string[] result = new string[] {
                                             Type.ToString(),
                                             Title,
                                             Counterparty,
                                             Company,
                                             Date,
                                             Number,
                                             Salary.ToString(),
                                             isUpd,
                                             Comment
            };
            return result;
        }


        internal protected virtual int GetDocType(string docTypeString)
        {
            return int.Parse(docTypeString);
        }

        internal protected virtual string GetDocCounterparty(string counterparty) => counterparty;

        internal protected string GetDocNumber(string docNumber)
        {
            string digitRus = "АВСЕНКМОРТХ";
            string digitEng = "ABCEHKMOPTX";
            string patternWord = @$"[{digitEng}]";
            string patternAddNumber = @"\~\d";
            string docNumberConverted;

            docNumberConverted = docNumber.ToUpper();
            docNumberConverted = Regex.Replace(docNumberConverted, patternAddNumber, string.Empty);
            docNumberConverted = Regex.Replace(docNumberConverted, @"\s+", string.Empty);
            docNumberConverted = Regex.IsMatch(docNumberConverted, patternWord) ? ReplaceDigitsFromEngToRus(docNumberConverted) : docNumberConverted;
            docNumberConverted = Regex.IsMatch(docNumberConverted, @"\w+") ? RemoveZeroInStartOfString(docNumberConverted) : docNumberConverted;

            string RemoveZeroInStartOfString(string docNumber)
            {
                return Regex.Replace(docNumber, @"^0+", string.Empty); ;
            }

            string ReplaceDigitsFromEngToRus(string docNumber)
            {
                string docNumberConverted = docNumber;
                for (var i = 0; i < digitEng.Length; i++)
                {
                    docNumberConverted = Regex.Replace(docNumberConverted, digitEng[i].ToString(), digitRus[i].ToString());
                }

                return docNumberConverted;
            }

            return docNumberConverted;
        }

        internal protected virtual float GetDocSalary(string docSalary)
        {
            string regexResult = Regex.Replace(docSalary, @"\.", @",");
            return float.Parse(regexResult);
        }
    }
}
