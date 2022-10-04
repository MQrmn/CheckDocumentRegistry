using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
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
        public Document() {}


        public Document(string[] docValues)
        {
            this.Type = Int32.Parse(docValues[0]);
            this.Title = docValues[1];
            this.Date = docValues[4];
            this.Counterparty = docValues[2];
            this.Number = docValues[5];
            this.Company = docValues[3];
            this.Salary = this.GetSalary(docValues[6]);
            if (docValues[7] == "Да") this.IsUpd = true;
            this.Comment = String.Empty;
        }


        public string[] GetArray()
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


        private float GetSalary(string salaryString)
        {
            string regexResult = Regex.Replace(salaryString, @"\.", @",");
            return float.Parse(regexResult);
        }


        private protected string SetDocNumber(string docNumber)
        {
            string digitRus = "АВСЕНКМОРТХ";
            string digitEng = "ABCEHKMOPTX";
            string patternWord = @$"[{digitEng}]";
            string patternAddNumber = @"\~\d";
            string docNumberConverted;

            docNumberConverted = docNumber.ToUpper();
            docNumberConverted = Regex.Replace(docNumberConverted, patternAddNumber, string.Empty);
            docNumberConverted = Regex.Replace(docNumberConverted, @"\s+", string.Empty);
            docNumberConverted = (Regex.IsMatch(docNumberConverted, patternWord) ? ReplaceWord(docNumberConverted) : docNumberConverted );
            docNumberConverted = (Regex.IsMatch(docNumberConverted, @"\w+") ? CutZero(docNumberConverted) : docNumberConverted );


            string CutZero(string docNumber)
            {
                return Regex.Replace(docNumber, @"^0+", string.Empty); ;
            }


            string ReplaceWord(string docNumber)
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
