using System.Text.RegularExpressions;

namespace RegComparator
{
    public class Document
    {
        public int Type { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Counterparty { get; set; }
        public string Number { get; set; }
        public string Date { get; set; }
        public float Salary { get; set; }
        public bool IsUpd { get; set; }
        public string? Comment { get; set; }
        public int StylePosition { get; set; }
        public bool Skip;

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
            Skip = false;
        }

        public string[] GetArray()
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


        public virtual int GetDocType(string docTypeString)
        {
            return int.Parse(docTypeString);
        }

        public virtual string GetDocCounterparty(string counterparty) => counterparty;

        public virtual string GetDocNumber(string docNumber)
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

        public virtual float GetDocSalary(string docSalary)
        {
            string regexResult = Regex.Replace(docSalary, @"\.", @",");
            return float.Parse(regexResult);
        }
    }
}
