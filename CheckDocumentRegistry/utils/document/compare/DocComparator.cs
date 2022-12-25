namespace RegComparator
{
    internal class DocComparator : IDocComparator
    {
        private List<Document> _matchedDocs1CUPPbuffer = new();
        private DocRepositoryBase _documents1CDO;
        private DocRepositoryBase _documentsRegistry;

        public DocComparator(DocRepositoryBase documents1CDO, DocRepositoryBase documentsRegistry)
        {
            _documents1CDO = documents1CDO;
            _documentsRegistry = documentsRegistry;

            this.ClearSourceByIgnore();
            this.CompareDocuments();
            this.ClearSourceByMatchedDocuments();

            //this.UnmatchedDocs1CDO = this.SourceDocs1CDO;
            //this.UnmatchedDocs1CUPP = this.SourceDocs1CUPP;
        }
        private void ClearSourceByIgnore()
        {
            Console.WriteLine("Подготовка списков документов для сравнения");

            // Clear 1C:DO documents source list by ignore list
            foreach (Document doc1CDO in this._documents1CDO.SkippedDocs)
            {
                this.RemoveSingleDocFromList(_documents1CDO.SourceDocs, doc1CDO);
            }

            // Clear 1C:UPP documents source list by ignore list
            foreach (Document doc1CUPP in _documentsRegistry.SkippedDocs)
            {
                this.RemoveSingleDocFromList(_documentsRegistry.SourceDocs, doc1CUPP);
            }
        }

        private void CompareDocuments()
        {
            Console.WriteLine("Сравнение документов");
            _documents1CDO.SourceDocs.ForEach(this.FindDocumentAddToMatchedSetUPD);

            // Matching related Upp documents
            foreach (Document doc1CUPP in _matchedDocs1CUPPbuffer)
            {
                this.FindUPDDocumentIn1CUPPAddToMatchList(doc1CUPP);
            }
        }

        private void ClearSourceByMatchedDocuments()
        {
            Console.WriteLine("Очистка списка документов в 1С:ДО");
            this.RemoveDocsFromList(_documents1CDO.SourceDocs, _documents1CDO.MatchedDocs);

            Console.WriteLine("Очистка списка документов в 1С:УПП");
            this.RemoveDocsFromList(_documentsRegistry.SourceDocs, _documentsRegistry.MatchedDocs);
        }

        // FIrst step of matching documents
        private void FindDocumentAddToMatchedSetUPD(Document doc1CDO)
        {
            _documentsRegistry.SourceDocs.ForEach(delegate(Document doc1CUPP)
             {
                bool isMatched = CompareSingleDocumentsAllFields(doc1CUPP, doc1CDO);

                if (isMatched)
                {
                    doc1CUPP.IsUpd = doc1CDO.IsUpd;

                    if (!_documents1CDO.MatchedDocs.Contains(doc1CDO))
                         _documents1CDO.MatchedDocs.Add(doc1CDO);

                    if(!_documentsRegistry.MatchedDocs.Contains(doc1CUPP))
                         _documentsRegistry.MatchedDocs.Add(doc1CUPP);

                    if (!this._matchedDocs1CUPPbuffer.Contains(doc1CUPP))
                        this._matchedDocs1CUPPbuffer.Add(doc1CUPP);
                }
             });
        }


        // Due to peculiarities in the "1C:UPP" configuration
        // UDP was entered as an invoice + bill of lading or an act of work performed
        // thus I find the second document from the PPM, which is included in the UPD
        private void FindUPDDocumentIn1CUPPAddToMatchList(Document doc1CUPPBuff)
        {
            foreach (Document doc1CUPPSrc in _documentsRegistry.SourceDocs)
            {
                bool isMatch = false;

                if (doc1CUPPBuff != doc1CUPPSrc && doc1CUPPBuff.IsUpd )
                    isMatch = this.CompareSingleDocumentsMainFields(doc1CUPPSrc, doc1CUPPBuff);

                if (isMatch)
                {
                    doc1CUPPSrc.IsUpd = true;

                    if (!_documentsRegistry.MatchedDocs.Contains(doc1CUPPSrc))
                        _documentsRegistry.MatchedDocs.Add(doc1CUPPSrc);
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
