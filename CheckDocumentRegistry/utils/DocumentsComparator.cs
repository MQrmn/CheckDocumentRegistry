
namespace CheckDocumentRegistry
{
    public class DocumentsComparator
    {
        public List<Document> doDocuments;
        public List<Document> uppDocuments;
        public List<Document> ignoreDoDocuments;
        public List<Document> ignoreUppDocuments;

        private List<Document>? firstMatchedUpp;

        public List<Document> documentsForDelete = new List<Document>();
        public List<Document> matchedDoDocuments = new List<Document>();
        public List<Document> matchedUppDocuments = new List<Document>();


        public DocumentsComparator(List<Document> inputDoDocs, List<Document> inputUppDocs, List<Document> IgnoreDo, List<Document> IgnoreUpp)
        {
            this.doDocuments = inputDoDocs;
            this.uppDocuments = inputUppDocs;
            this.ignoreDoDocuments = IgnoreDo;
            this.ignoreUppDocuments = IgnoreUpp;

            Console.WriteLine("Подготовка списков документов для сравнения");

            // Clear 1C:DO documents list by ignore list
            foreach (Document document in this.ignoreDoDocuments)
            {
                this.RemoveDocumentFromSource(this.doDocuments, document);
            }

            // Clear 1C:UPP documents list by ignore list
            foreach (Document document in this.ignoreUppDocuments)
            {
                this.RemoveDocumentFromSource(this.uppDocuments, document);
            }

            
            Console.WriteLine("Сравнение документов");

            this.doDocuments.ForEach(this.Compare);


            // Matching related Upp documents
            foreach (Document document in this.firstMatchedUpp)
            {
                this.CatchRalatedUppDocument(document);
            }


            Console.WriteLine("Очистка списка документов в 1С:ДО");
            this.ClearDocumentList(this.doDocuments, this.matchedDoDocuments);

            Console.WriteLine("Очистка списка документов в 1С:УПП");
            this.ClearDocumentList(this.uppDocuments, this.matchedUppDocuments);

        }

        private void Compare(Document doDocument)
        {
            this.firstMatchedUpp = this.uppDocuments.FindAll(delegate(Document document)
             {
                bool result = CompareDocuments(document, doDocument);

                if (result)
                {
                    if (doDocument.isUpd) document.isUpd = doDocument.isUpd;

                    this.matchedDoDocuments.Add(doDocument);
                    this.matchedUppDocuments.Add(document);
                }

                return result;

             });
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

        // Clear source document list by matched documents
        private void ClearDocumentList(List<Document> sourceDocumentList, List<Document> catchedDocumentList)
        {
            catchedDocumentList.ForEach(delegate(Document document)
            {
                sourceDocumentList.Remove(document);
            });
        }

        // Remove one document from list
        private void RemoveDocumentFromSource(List<Document> documents, Document ignDocument)
        {
            Document? documentForRemove = documents.Find(delegate (Document document)
            {
                return CompareDocuments(document, ignDocument);
            });

            if (documentForRemove is not null) this.doDocuments.Remove(documentForRemove);

        }

        // Comparing two documents
        internal bool CompareDocuments(Document firstDocument, Document secondDocument)
        {
            bool result = firstDocument.docType == secondDocument.docType
                            && firstDocument.docNumber == secondDocument.docNumber
                            && firstDocument.docSum == secondDocument.docSum
                            && firstDocument.docDate == secondDocument.docDate;
            return result;

        }

    }
}
