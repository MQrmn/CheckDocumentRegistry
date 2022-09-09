using System;
using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    public class DoDocument
    {
        public int docType { get; }
        public string docTitle { get; }
        public string docCompany { get; }
        public string docCounterparty { get; }
        public string docNumber { get; }
        public string docDate { get; }
        public float docSum { get; }
        public bool isUpd { get; }


        public DoDocument(string[] docValues)
        {

            this.docType = this.GetDocType(docValues[0]);
            this.docTitle = docValues[1];
            this.docCompany = docValues[2];
            this.docCounterparty = docValues[3];
            this.docNumber = docValues[4];
            this.docDate = docValues[5];
            this.docSum = this.GetDocSum(docValues[6]);
            if (docValues[7] != null) this.isUpd = true;

        }

        int GetDocType(string rawDocType)
        {
            return rawDocType switch
            {
                "Приобретение товаров и услуг" => 1,
                "Входящая Счет-Фактура" => 2,
                "Входящий Акт выполненных работ" => 3,
                _ => 0
            };
        }

        float GetDocSum(string stringSum)
        {
            string pattern = @"[A-Z\s]";
            string regexResult = Regex.Replace(stringSum, pattern, String.Empty, RegexOptions.IgnoreCase);
            return float.Parse(regexResult);
        }
        

    }
}
