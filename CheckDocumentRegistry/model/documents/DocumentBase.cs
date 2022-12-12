using System.Text.RegularExpressions;

namespace RegComparator
{
    public abstract class DocumentBase
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


        public DocumentBase() 
        {
        }

        public DocumentBase(string[] docFields, int[] docFieldsIndex)
        {
            Type = GetDocType(docFields[docFieldsIndex[0]]);
            Title = docFields[docFieldsIndex[1]];
            Counterparty = GetDocCounterparty(docFields[docFieldsIndex[2]]);
            Company = docFields[docFieldsIndex[3]];
            Date = docFields[docFieldsIndex[4]];
            Number = GetDocNumber(docFields[docFieldsIndex[5]]);
            Salary = GetDocSalary(docFields[docFieldsIndex[6]]);
            if (docFields[docFieldsIndex[docFieldsIndex.Length - 1]] == "Да") IsUpd = true;
            Comment = String.Empty;
        }

        public DocumentBase(string[] docFields)
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
        public abstract int GetDocType(string docTypeString);
        public abstract string GetDocCounterparty(string counterparty);
        public abstract string GetDocNumber(string docNumberString);
        public abstract float GetDocSalary(string docSalary);
    }
}
