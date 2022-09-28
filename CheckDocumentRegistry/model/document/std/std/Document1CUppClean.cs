
namespace CheckDocumentRegistry
{
    public class Document1CUppClean : Document1CUppStandard
    {
        public Document1CUppClean(string[] docValues)
        {
            this.Type = this.GetDocType(docValues[0]);
            this.Title = docValues[1];
            this.Date = docValues[2];
            this.Counterparty = docValues[3];
            this.Number = this.SetDocNumber(docValues[4]);
            this.Company = docValues[5];

            if (docValues[6] != String.Empty)
                this.Salary = this.GetDocSum(docValues[6]);
        }
    }
}
