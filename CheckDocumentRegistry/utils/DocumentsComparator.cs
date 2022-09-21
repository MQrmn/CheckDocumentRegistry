
namespace CheckDocumentRegistry
{
    public class DocumentsComparator
    {
        public List<Document> doDocuments;
        public List<Document> uppDocuments;
        public List<Document> ignoreDoDocuments;

        public List<Document> matchedDoDocuments = new List<Document>();
        public List<Document> matchedUppDocuments = new List<Document>();


        public DocumentsComparator(List<Document> inputDoDocs, List<Document> inputUppDocs, List<Document> inputIgnoreDo)
        {
            this.doDocuments = inputDoDocs;
            this.uppDocuments = inputUppDocs;
            this.ignoreDoDocuments = inputIgnoreDo;
            this.Compare();
        }

        public void Compare()
        {
            Console.WriteLine("Preparing DO Documents list for comparing");
            this.ClearSourseDoListByIgnore(this.doDocuments, this.ignoreDoDocuments);

            Console.WriteLine("Comparing documents");
            this.doDocuments.ForEach(this.CompareOneDocument);

            Console.WriteLine("Preparing list of uncatched documents in Do");
            this.ClearSourseDoList(this.doDocuments, this.matchedDoDocuments);

            Console.WriteLine("Preparing list of uncatched documents in Upp");
            this.ClearSourseUppList(this.uppDocuments, this.matchedUppDocuments);
        }

        private void CompareOneDocument(Document doDocument)
        {
            List<Document> firstCatchedUpp = this.uppDocuments.FindAll(
             delegate(Document document)
             {
                 return CatchDoDocumentInUpp(document, doDocument);
             });

            firstCatchedUpp.ForEach(delegate (Document document)
            {
                this.CatchRalatedUppDocument(document);
            });
        }
        
        private bool CatchDoDocumentInUpp(Document uppDocument, Document doDocument)
        {
            bool result = uppDocument.docType == doDocument.docType
                && uppDocument.docNumber == doDocument.docNumber
                && uppDocument.docSum == doDocument.docSum;

            if (result)
            {
                if (doDocument.isUpd) uppDocument.isUpd = doDocument.isUpd;

                this.matchedDoDocuments.Add(doDocument);
                this.matchedUppDocuments.Add(uppDocument);
            }
            return result;
        }

        private void CatchRalatedUppDocument(Document catchedUppDocument)
        {
            List<Document> result = this.uppDocuments.FindAll(
                delegate(Document document)
                {
                    bool result = document.docType != catchedUppDocument.docType
                            && document.docNumber == catchedUppDocument.docNumber
                            && document.docSum == catchedUppDocument.docSum
                            && catchedUppDocument.isUpd == true;

                    if (result) document.isUpd = true;

                    return result;
                });

            result.ForEach(delegate (Document document)
            {
                this.matchedUppDocuments.Add(document);
            });
        }

        private void ClearSourseDoList(List<Document> sourceDocumentList, List<Document> catchedDocumentList)
        {
            catchedDocumentList.ForEach(delegate(Document document)
            {
                sourceDocumentList.Remove(document);
            });
        }

        private void ClearSourseUppList(List<Document> sourceDocumentList, List<Document> catchedDocumentList)
        {
            catchedDocumentList.ForEach(delegate (Document document)
            {
                sourceDocumentList.Remove(document);
            });
        }

        private void ClearSourseDoListByIgnore(List<Document> sourceDocumentList, List<Document> ignoreDocumentList)
        {

        }

    }
}
