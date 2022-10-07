
namespace CheckDocumentRegistry
{

    internal class FullDocumentsComparator
    {
        internal List<Document> MatchedDocuments1CDO = new List<Document>();  // Documents from 1C:Document Management that coincided with 1C:UPP documents
        internal List<Document> MatchedDocuments1CUPP = new List<Document>(); // Documents from 1C:UPP that coincided with 1C:Document Management documents
        internal List<Document> UnmatchedDocuments1CDO;                       // Documents from 1C:Document Management that not coincided with 1C:UPP documents
        internal List<Document> UnmatchedDocuments1CUPP;                      // Documents from 1C:UPP that not coincided with 1C:Document Management documents
        internal List<Document> SourceDocuments1CDO;                          // Source documents in 1C:Document Management
        internal List<Document> SourceDocuments1CUPP;                         // Source documents in 1C:UPP
        private List<Document> ignoreDocuments1CDO;                           // Ignored documents in 1C:Document Management
        private List<Document> ignoreDocuments1CUPP;                          // Ignored documents in 1C:UPP
        private List<Document> bufferMatchedDocuments1CUPP;


        internal FullDocumentsComparator(List<Document> DocumentsDo, List<Document> DocumentsUpp, List<Document> DocumentsDoIgnore, List<Document> DocumentsUppIgnore)
        {
            this.SourceDocuments1CDO = DocumentsDo;
            this.SourceDocuments1CUPP = DocumentsUpp;
            this.ignoreDocuments1CDO = DocumentsDoIgnore;
            this.ignoreDocuments1CUPP = DocumentsUppIgnore;
            this.bufferMatchedDocuments1CUPP = new List<Document>();

            this.ClearSourceByIgnore();
            this.CompareDocuments();
            this.ClearSourceByMatchedDocuments();

            this.UnmatchedDocuments1CDO = this.SourceDocuments1CDO;
            this.UnmatchedDocuments1CUPP = this.SourceDocuments1CUPP;
        }

        private void ClearSourceByMatchedDocuments()
        {
            Console.WriteLine("Очистка списка документов в 1С:ДО");
            this.ClearDocumentsByList(this.SourceDocuments1CDO, this.MatchedDocuments1CDO);

            Console.WriteLine("Очистка списка документов в 1С:УПП");
            this.ClearDocumentsByList(this.SourceDocuments1CUPP, this.MatchedDocuments1CUPP);
        }

        private void CompareDocuments()
        {
            Console.WriteLine("Сравнение документов");
            this.SourceDocuments1CDO.ForEach(this.FindDocumentAddToMatchedSetUPD);

            // Matching related Upp documents
            foreach (Document uppDocument in this.bufferMatchedDocuments1CUPP)
            {
                this.FindUPDDocumentInUppAddToMatchList(uppDocument);
            }
        }

        private void ClearSourceByIgnore()
        {
            Console.WriteLine("Подготовка списков документов для сравнения");

            // Clear 1C:DO documents source list by ignore list
            foreach (Document document in this.ignoreDocuments1CDO)
            {
                this.FindRemoveDocument(this.SourceDocuments1CDO, document);
            }

            // Clear 1C:UPP documents source list by ignore list
            foreach (Document document in this.ignoreDocuments1CUPP)
            {
                this.FindRemoveDocument(this.SourceDocuments1CUPP, document);
            }
        }

        // FIrst step of matching documents
        private void FindDocumentAddToMatchedSetUPD(Document documentDo)
        {
            this.SourceDocuments1CUPP.ForEach(delegate(Document documentUpp)
             {
                bool isMatched = CompareSingleDocumentsAllFields(documentUpp, documentDo);

                if (isMatched)
                {
                    documentUpp.IsUpd = documentDo.IsUpd;

                    if (!this.MatchedDocuments1CDO.Contains(documentDo))
                        this.MatchedDocuments1CDO.Add(documentDo);

                    if(!this.MatchedDocuments1CUPP.Contains(documentUpp))
                        this.MatchedDocuments1CUPP.Add(documentUpp);

                    if (!this.bufferMatchedDocuments1CUPP.Contains(documentUpp))
                        this.bufferMatchedDocuments1CUPP.Add(documentUpp);
                }
             });
        }


        // Due to peculiarities in the "1C:UPP" configuration
        // UDP was entered as an invoice + bill of lading or an act of work performed
        // thus I find the second document from the PPM, which is included in the UPD
        private void FindUPDDocumentInUppAddToMatchList(Document matchedUppDocument)
        {
            foreach (Document unmatchedUppDocument in this.SourceDocuments1CUPP)
            {
                bool isMatch = false;

                if (matchedUppDocument != unmatchedUppDocument && matchedUppDocument.IsUpd )
                    isMatch = this.CompareSingleDocumentsMainFields(unmatchedUppDocument, matchedUppDocument);

                if (isMatch)
                {
                    unmatchedUppDocument.IsUpd = true;

                    if (!this.MatchedDocuments1CUPP.Contains(unmatchedUppDocument))
                        this.MatchedDocuments1CUPP.Add(unmatchedUppDocument);
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
                return CompareSingleDocumentsAllFields(document, ignDocument);
            });

            this.ClearDocumentsByList(sourceDocuments, documentsForRemove);

        }


        // Comparing two documents
        private bool CompareSingleDocumentsAllFields(Document firstDocument,
                                        Document secondDocument)
        {
            bool compareResult = this.CompareSingleDocumentsMainFields(firstDocument, secondDocument);

            if (!compareResult) return compareResult;
            else return firstDocument.Type == secondDocument.Type; ;
        }
          

        private bool CompareSingleDocumentsMainFields(Document firstDocument,
                                        Document secondDocument)
        {
            return firstDocument.Number == secondDocument.Number
                            && firstDocument.Salary == secondDocument.Salary
                            && firstDocument.Date == secondDocument.Date;
        }
    }
}
