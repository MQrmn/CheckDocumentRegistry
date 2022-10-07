using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    public class Document
    {
        internal int Type { get; set; }
        internal string Title { get; set; }
        internal string Company { get; set; }
        internal string Counterparty { get; set; }
        internal string Number { get; set; }
        internal string Date { get; set; }
        internal float Salary { get; set; }
        internal bool IsUpd { get; set; }
        internal string? Comment { get; set; }
        internal int StylePosition { get; set; }


        public Document() {}


        internal Document(string[] docFields)
        {
            this.Type = this.GetDocType(docFields[0]);
            this.Title = docFields[1];
            this.Counterparty = this.GetCounterParty(docFields[2]);
            this.Company = docFields[3];
            this.Date = docFields[4];
            this.Number = this.GetDocNumber(docFields[5]);
            this.Salary = this.GetDocSalary(docFields[6]);

            if (docFields[7] == "Да") this.IsUpd = true;
            this.Comment = String.Empty;
        }


        internal string[] GetArray()
        {
            string isUpd = this.IsUpd ? "Да" : "Нет";

            string[] result = new string[] {
                                             this.Type.ToString(),
                                             this.Title,
                                             this.Counterparty,
                                             this.Company,
                                             this.Date,
                                             this.Number,
                                             this.Salary.ToString(),
                                             isUpd,
                                             this.Comment
            };
            return result;
        }

        private int GetDocType(string docTypeString)
        {
            return Int32.Parse(docTypeString);
        }

        private string GetCounterParty(string counterparty) => counterparty;



        private float GetDocSalary(string docSalary)
        {
            string regexResult = Regex.Replace(docSalary, @"\.", @",");
            return float.Parse(regexResult);
        }



        private protected string GetDocNumber(string docNumber)
        {

            string digitRus = "АВСЕНКМОРТХ";
            string digitEng = "ABCEHKMOPTX";
            string patternWord = @$"[{digitEng}]";
            string patternAddNumber = @"\~\d";
            string docNumberConverted;

            docNumberConverted = docNumber.ToUpper();
            docNumberConverted = Regex.Replace(docNumberConverted, patternAddNumber, string.Empty);
            docNumberConverted = Regex.Replace(docNumberConverted, @"\s+", string.Empty);
            docNumberConverted = (Regex.IsMatch(docNumberConverted, patternWord) ? ReplaceDigitsFromEngToRus(docNumberConverted) : docNumberConverted );
            docNumberConverted = (Regex.IsMatch(docNumberConverted, @"\w+") ? RemoveZeroInStartOfString(docNumberConverted) : docNumberConverted );


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
    }
}
