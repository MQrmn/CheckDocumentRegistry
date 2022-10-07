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
        


        public Document() 
        {
            
        }


        internal Document(string[] docFields, int[] docFieldsIndex)
        {
            this.Type = this.GetDocType(docFields[docFieldsIndex[0]]);
            this.Title = docFields[docFieldsIndex[1]];
            this.Counterparty = this.GetCounterParty(docFields[docFieldsIndex[2]]);
            this.Company = docFields[docFieldsIndex[3]];
            this.Date = docFields[docFieldsIndex[4]];
            this.Number = this.GetDocNumber(docFields[docFieldsIndex[5]]);
            this.Salary = this.GetDocSalary(docFields[docFieldsIndex[6]]);

            if (docFields[docFields.Length - 1] == "Да") this.IsUpd = true;
            this.Comment = String.Empty;
        }


        internal Document(string[] docFields)
        {
            
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


        private protected virtual int GetDocType(string docTypeString)
        {
            return Int32.Parse(docTypeString);
        }


        private protected virtual string GetCounterParty(string counterparty) => counterparty;


        private protected virtual float GetDocSalary(string docSalary)
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
