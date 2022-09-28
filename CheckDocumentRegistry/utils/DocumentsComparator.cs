
namespace CheckDocumentRegistry
{
    public class DocumentsComparator
    {
        public List<Document> Documents1CDoMatched = new List<Document>();
        public List<Document> Documents1CUppMatched = new List<Document>();
        public List<Document> Documents1CDoUnmatched;
        public List<Document> Documents1CUppUnmatched;

        public List<Document> documents1CDoSource;
        public List<Document> documents1CUppSource;
        private List<Document> ignoreDoDocuments;
        private List<Document> ignoreUppDocuments;

        private List<Document>? matchedUppDocumentsBuffer;

        private enum CompareMode
        {
            Basic,
            FindUpd
        }

        public DocumentsComparator(List<Document> inputDoDocs, List<Document> inputUppDocs, List<Document> IgnoreDo, List<Document> IgnoreUpp)
        {
            this.documents1CDoSource = inputDoDocs;
            this.documents1CUppSource = inputUppDocs;
            this.ignoreDoDocuments = IgnoreDo;
            this.ignoreUppDocuments = IgnoreUpp;

            Console.WriteLine("Подготовка списков документов для сравнения");

            // Clear 1C:DO documents source list by ignore list
            foreach (Document document in this.ignoreDoDocuments)
            {
                this.FindRemoveDocument(this.documents1CDoSource, document);
            }

            // Clear 1C:UPP documents source list by ignore list
            foreach (Document document in this.ignoreUppDocuments)
            {
                this.FindRemoveDocument(this.documents1CUppSource, document);
            }
            
            Console.WriteLine("Сравнение документов");
            this.documents1CDoSource.ForEach(this.FindDocumentAddToMatched);

            // Matching related Upp documents
            foreach (Document document in this.matchedUppDocumentsBuffer)
            {
                this.FindUpdDocumentInUpp(document);
            }

            Console.WriteLine("Очистка списка документов в 1С:ДО");
            this.ClearDocumentsByList(this.documents1CDoSource, this.Documents1CDoMatched);

            Console.WriteLine("Очистка списка документов в 1С:УПП");
            this.ClearDocumentsByList(this.documents1CUppSource, this.Documents1CUppMatched);

            this.Documents1CDoUnmatched = this.documents1CDoSource;
            this.Documents1CUppUnmatched = this.documents1CUppSource;

        }

        private void FindDocumentAddToMatched(Document documentDo)
        {
            this.matchedUppDocumentsBuffer = this.documents1CUppSource.FindAll(delegate(Document documentUpp)
             {
                bool matched = CompareSingleDocuments(documentUpp, documentDo, CompareMode.Basic);

                if (matched)
                {
                    if (documentDo.isUpd) documentUpp.isUpd = documentDo.isUpd;

                    this.Documents1CDoMatched.Add(documentDo);
                    this.Documents1CUppMatched.Add(documentUpp);
                }

                return matched;

             });
        }

        // Due to peculiarities in the "1C:UPP" configuration
        // UDP was entered as an invoice + bill of lading or an act of work performed
        // thus I find the second document from the PPM, which is included in the UPD
        private void FindUpdDocumentInUpp(Document catchedUppDocument)
        {
            foreach (Document document in this.documents1CUppSource)
            {
                bool match = this.CompareSingleDocuments(document, catchedUppDocument, CompareMode.FindUpd);

                if (match)
                {
                    document.isUpd = true;
                    this.Documents1CUppMatched.Add(document);
                }
            }
        }

        // Clear source document list by matched documents
        private void ClearDocumentsByList(List<Document> sourceDocumentList, List<Document> catchedDocumentList)
        {
            catchedDocumentList.ForEach(delegate(Document document)
            {
                sourceDocumentList.Remove(document);
            });
        }

        // Remove one document from list
        private void FindRemoveDocument(List<Document> sourceDocuments, Document ignDocument)
        {
            List<Document> documentsForRemove = sourceDocuments.FindAll(delegate (Document document)
            {
                return CompareSingleDocuments(document, ignDocument, CompareMode.Basic);
            });

            this.ClearDocumentsByList(sourceDocuments, documentsForRemove);

        }

        // Comparing two documents
        private bool CompareSingleDocuments(  Document firstDocument, 
                                        Document secondDocument, 
                                        CompareMode compareMode)
        {
            bool result = firstDocument.docNumber == secondDocument.docNumber
                            && firstDocument.docSum == secondDocument.docSum
                            && firstDocument.docDate == secondDocument.docDate;

            if (!result) return result;

            if (compareMode == CompareMode.Basic)
            {
                return firstDocument.docType == secondDocument.docType;
            }
            else
            {
                return firstDocument.docType != secondDocument.docType 
                    && secondDocument.isUpd == true;
            }
        }
    }
}
