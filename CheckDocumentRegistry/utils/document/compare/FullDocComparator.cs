
namespace CheckDocumentRegistry
{

    internal class FullDocComparator
    {
        internal List<Document> MatchedDocs1CDO = new List<Document>();  // Documents from 1C:Document Management that coincided with 1C:UPP documents
        internal List<Document> MatchedDocs1CUPP = new List<Document>(); // Documents from 1C:UPP that coincided with 1C:Document Management documents
        internal List<Document> UnmatchedDocs1CDO;                       // Documents from 1C:Document Management that not coincided with 1C:UPP documents
        internal List<Document> UnmatchedDocs1CUPP;                      // Documents from 1C:UPP that not coincided with 1C:Document Management documents
        internal List<Document> SourceDocs1CDO;                          // Source documents in 1C:Document Management
        internal List<Document> SourceDocs1CUPP;                         // Source documents in 1C:UPP
        private List<Document> passDocs1CDO;                             // Ignored documents in 1C:Document Management
        private List<Document> passDocs1CUPP;                            // Ignored documents in 1C:UPP
        private List<Document> matchedDocs1CUPPbuffer;  


        internal FullDocComparator(List<Document> docs1CDo, List<Document> docs1CUPP, List<Document> docsPass1CDO, List<Document> docsPassUPP)
        {
            this.SourceDocs1CDO = docs1CDo;
            this.SourceDocs1CUPP = docs1CUPP;
            this.passDocs1CDO = docsPass1CDO;
            this.passDocs1CUPP = docsPassUPP;
            this.matchedDocs1CUPPbuffer = new List<Document>();

            this.ClearSourceByIgnore();
            this.CompareDocuments();
            this.ClearSourceByMatchedDocuments();

            this.UnmatchedDocs1CDO = this.SourceDocs1CDO;
            this.UnmatchedDocs1CUPP = this.SourceDocs1CUPP;
        }

        private void ClearSourceByMatchedDocuments()
        {
            Console.WriteLine("Очистка списка документов в 1С:ДО");
            this.RemoveDocsFromList(this.SourceDocs1CDO, this.MatchedDocs1CDO);

            Console.WriteLine("Очистка списка документов в 1С:УПП");
            this.RemoveDocsFromList(this.SourceDocs1CUPP, this.MatchedDocs1CUPP);
        }

        private void CompareDocuments()
        {
            Console.WriteLine("Сравнение документов");
            this.SourceDocs1CDO.ForEach(this.FindDocumentAddToMatchedSetUPD);

            // Matching related Upp documents
            foreach (Document doc1CUPP in this.matchedDocs1CUPPbuffer)
            {
                this.FindUPDDocumentIn1CUPPAddToMatchList(doc1CUPP);
            }
        }

        private void ClearSourceByIgnore()
        {
            Console.WriteLine("Подготовка списков документов для сравнения");

            // Clear 1C:DO documents source list by ignore list
            foreach (Document doc1CDO in this.passDocs1CDO)
            {
                this.RemoveSingleDocFromList(this.SourceDocs1CDO, doc1CDO);
            }

            // Clear 1C:UPP documents source list by ignore list
            foreach (Document doc1CUPP in this.passDocs1CUPP)
            {
                this.RemoveSingleDocFromList(this.SourceDocs1CUPP, doc1CUPP);
            }
        }

        // FIrst step of matching documents
        private void FindDocumentAddToMatchedSetUPD(Document doc1CDO)
        {
            this.SourceDocs1CUPP.ForEach(delegate(Document doc1CUPP)
             {
                bool isMatched = CompareSingleDocumentsAllFields(doc1CUPP, doc1CDO);

                if (isMatched)
                {
                    doc1CUPP.IsUpd = doc1CDO.IsUpd;

                    if (!this.MatchedDocs1CDO.Contains(doc1CDO))
                        this.MatchedDocs1CDO.Add(doc1CDO);

                    if(!this.MatchedDocs1CUPP.Contains(doc1CUPP))
                        this.MatchedDocs1CUPP.Add(doc1CUPP);

                    if (!this.matchedDocs1CUPPbuffer.Contains(doc1CUPP))
                        this.matchedDocs1CUPPbuffer.Add(doc1CUPP);
                }
             });
        }


        // Due to peculiarities in the "1C:UPP" configuration
        // UDP was entered as an invoice + bill of lading or an act of work performed
        // thus I find the second document from the PPM, which is included in the UPD
        private void FindUPDDocumentIn1CUPPAddToMatchList(Document doc1CUPPBuff)
        {
            foreach (Document doc1CUPPSrc in this.SourceDocs1CUPP)
            {
                bool isMatch = false;

                if (doc1CUPPBuff != doc1CUPPSrc && doc1CUPPBuff.IsUpd )
                    isMatch = this.CompareSingleDocumentsMainFields(doc1CUPPSrc, doc1CUPPBuff);

                if (isMatch)
                {
                    doc1CUPPSrc.IsUpd = true;

                    if (!this.MatchedDocs1CUPP.Contains(doc1CUPPSrc))
                        this.MatchedDocs1CUPP.Add(doc1CUPPSrc);
                }
            }
        }


        // Remove one document from list
        private void RemoveSingleDocFromList(List<Document> docList, Document docRemove)
        {
            List<Document> docRemoveList = docList.FindAll(delegate (Document document)
            {
                return CompareSingleDocumentsAllFields(document, docRemove);
            });
            this.RemoveDocsFromList(docList, docRemoveList);
        }


        // Clear source document list by matched documents
        private void RemoveDocsFromList(List<Document> docsSrc, List<Document> docRemoveList)
        {
            docRemoveList.ForEach(delegate (Document docRemove)
            {
                docsSrc.Remove(docRemove);
            });
        }


        // Comparing two documents
        private bool CompareSingleDocumentsAllFields(Document docFirst, Document docsecond)
        {
            bool isMainFieldsMatch = this.CompareSingleDocumentsMainFields(docFirst, docsecond);

            if (!isMainFieldsMatch) return isMainFieldsMatch;
            else return docFirst.Type == docsecond.Type;
        }
          

        private bool CompareSingleDocumentsMainFields(Document docFirst, Document docsecond)
        {
            return docFirst.Number == docsecond.Number
                            && docFirst.Salary == docsecond.Salary
                            && docFirst.Date == docsecond.Date;
        }
    }
}
