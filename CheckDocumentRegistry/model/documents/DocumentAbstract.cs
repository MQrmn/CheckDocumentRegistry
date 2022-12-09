using System.Text.RegularExpressions;

namespace RegComparator
{
    public abstract class DocumentAbstract
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


        public DocumentAbstract() 
        {
        }

        public DocumentAbstract(string[] docFields, int[] docFieldsIndex)
        {
            this.Type = this.GetDocType(docFields[docFieldsIndex[0]]);
            this.Title = docFields[docFieldsIndex[1]];
            this.Counterparty = this.GetDocCounterparty(docFields[docFieldsIndex[2]]);
            this.Company = docFields[docFieldsIndex[3]];
            this.Date = docFields[docFieldsIndex[4]];
            this.Number = this.GetDocNumber(docFields[docFieldsIndex[5]]);
            this.Salary = this.GetDocSalary(docFields[docFieldsIndex[6]]);

            if (docFields[docFieldsIndex[docFieldsIndex.Length - 1]] == "Да") this.IsUpd = true;
            this.Comment = String.Empty;
        }

        public DocumentAbstract(string[] docFields)
        {}
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
        abstract public int GetDocType(string docTypeString);
        abstract public string GetDocCounterparty(string counterparty);
        abstract public string GetDocNumber(string docNumberString);
        abstract public float GetDocSalary(string docSalary);
    }
}
