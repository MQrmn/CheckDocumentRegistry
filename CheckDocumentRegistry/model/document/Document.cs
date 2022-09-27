using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    public class Document
    {
        public int docType { get; set; }
        public string docTitle { get; set; }
        public string docCompany { get; set; }
        public string docCounterparty { get; set; }
        public string docNumber { get; set; }
        public string docDate { get; set; }
        public float docSum { get; set; }
        public bool isUpd { get; set; }

        public Document() {}

        public Document(string[] docValues)
        {
            this.docType = Int32.Parse(docValues[0]);
            this.docTitle = docValues[1];
            this.docDate = docValues[4];
            this.docCounterparty = docValues[2];
            this.docNumber = docValues[5];
            this.docCompany = docValues[3];
            Console.WriteLine(docValues[6]);
            this.docSum = float.Parse(docValues[6]);
            if (docValues[7] == "Да") this.isUpd = true;
        }

        public string[] GetArray()
        {
            string isUpd = this.isUpd ? "Да" : "Нет";

            string[] result = new string[] {
                                             this.docType.ToString(),
                                             this.docTitle,
                                             this.docCounterparty,
                                             this.docCompany,
                                             this.docDate,
                                             this.docNumber,
                                             this.docSum.ToString(),
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
