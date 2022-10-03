
namespace CheckDocumentRegistry
{

    public class FullDocumentsComparator
    {
        public List<Document> Documents1CDoMatched = new List<Document>();  // Documents from 1C:Document Management that coincided with 1C:UPP documents
        public List<Document> Documents1CUppMatched = new List<Document>(); // Documents from 1C:UPP that coincided with 1C:Document Management documents
        public List<Document> Documents1CDoUnmatched;                       // Documents from 1C:Document Management that not coincided with 1C:UPP documents
        public List<Document> Documents1CUppUnmatched;                      // Documents from 1C:UPP that not coincided with 1C:Document Management documents

        public List<Document> documents1CDoSource;                          // Source documents in 1C:Document Management
        public List<Document> documents1CUppSource;                         // Source documents in 1C:UPP
        private List<Document> ignoreDoDocuments;                           // Ignored documents in 1C:Document Management
        private List<Document> ignoreUppDocuments;                          // Ignored documents in 1C:UPP

        private List<Document>? matchedUppDocumentsBuffer;

        private enum CompareMode
        {
            Basic,
            FindUpd
        }


        public FullDocumentsComparator(List<Document> DocumentsDo, List<Document> DocumentsUpp, List<Document> DocumentsDoIgnore, List<Document> DocumentsUppIgnore)
        {
            this.documents1CDoSource = DocumentsDo;
            this.documents1CUppSource = DocumentsUpp;
            this.ignoreDoDocuments = DocumentsDoIgnore;
            this.ignoreUppDocuments = DocumentsUppIgnore;

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
                    if (documentDo.IsUpd) documentUpp.IsUpd = documentDo.IsUpd;

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
                    document.IsUpd = true;
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
            bool result = firstDocument.Number == secondDocument.Number
                            && firstDocument.Salary == secondDocument.Salary
                            && firstDocument.Date == secondDocument.Date;

            if (!result) return result;

            if (compareMode == CompareMode.Basic)
            {
                return firstDocument.Type == secondDocument.Type;
            }
            else
            {
                return firstDocument.Type != secondDocument.Type 
                    && secondDocument.IsUpd == true;
            }
        }
    }
}
