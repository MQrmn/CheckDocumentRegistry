

namespace CheckDocumentRegistry
{
    public class Document1CDoClean : Document1CDoStandard
    {
        
        public Document1CDoClean(string[] docValues)
        {

            this.Type = this.GetDocType(docValues[0]);
            this.Title = docValues[1];
            this.Company = docValues[2];
            this.Counterparty = this.GetDocCounterparty(docValues[3]);
            this.Number = this.SetDocNumber(docValues[4]);
            this.Date = docValues[5];

            if (docValues[6] != String.Empty)
                this.Salary = this.GetDocSum(docValues[6]);

            if (docValues[7] == "Да") this.IsUpd = true;
        }
    }
}
