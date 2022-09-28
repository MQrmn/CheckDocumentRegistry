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

        public Document() {}

        public Document(string[] docValues)
        {
            this.Type = Int32.Parse(docValues[0]);
            this.Title = docValues[1];
            this.Date = docValues[4];
            this.Counterparty = docValues[2];
            this.Number = docValues[5];
            this.Company = docValues[3];
            this.Salary = float.Parse(docValues[6]);
            if (docValues[7] == "Да") this.IsUpd = true;
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
                                             isUpd
            };
            return result;
        }
        protected string SetDocNumber(string input)
        {
            string digitRus = "АВСЕНКМОРТХ";
            string digitEng = "ABCEHKMOPTX";
            string patternWord = @$"[{digitEng}]";
            string patternAddNumber = @"\~\d";
            string result;

            result = input.ToUpper();
            result = Regex.Replace(result, patternAddNumber, string.Empty);
            result = Regex.Replace(result, @"\s+", string.Empty);
            result = (Regex.IsMatch(result, patternWord) ? ReplaceWord(result) : result );
            result = (Regex.IsMatch(result, @"\w+") ? CutZero(result) : result );

            string CutZero(string input)
            {
                return Regex.Replace(input, @"^0+", string.Empty); ;
            }

            string ReplaceWord(string input)
            {
                string result = input;

                for (var i = 0; i < digitEng.Length; i++)
                {
                    result = Regex.Replace(result, digitEng[i].ToString(), digitRus[i].ToString());

                }

                return result;
            }

            return result;
        }
    }


}
