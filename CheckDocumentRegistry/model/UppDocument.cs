
namespace CheckDocumentRegistry
{
    internal class UppDocument
    {
        public string docDate { get; }
        public int docType { get; }
        public string docCompany { get; }
        public string docCounterparty { get; }
        public string docNumber { get; }
        public string docSum { get; }
        public bool isUpd { get; set; }
         
        public UppDocument(string[] docValues)
        {
            this.docDate = docValues[1];
            this.docType = this.GetDocType(docValues[3]);
            this.docCompany = docValues[5];
            this.docCounterparty = docValues[6];
            this.docNumber = docValues[2];
            this.docSum = docValues[7]; 
        }

        int GetDocType(string input)
        {
            return input switch
            {
                "Поступление товаров и услуг" => 1,
                "Счет-фактура полученный" => 2,
                _ => 0

            };
        }


    }
}
