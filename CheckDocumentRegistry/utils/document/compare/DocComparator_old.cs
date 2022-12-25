namespace RegComparator
{
    internal class DocComparator_old
    {
        private List<Document> _matchedDocs1CUPPbuffer = new();
        private DocRepositoryBase _documents1CDO;
        private DocRepositoryBase _documentsRegistry;

        public DocComparator_old(DocRepositoryBase documents1CDO, DocRepositoryBase documentsRegistry)
        {
            _documents1CDO = documents1CDO;
            _documentsRegistry = documentsRegistry;

            ClearSrcBySkip(_documents1CDO.SkippedDocs, _documents1CDO.SourceDocs);
            ClearSrcBySkip(_documentsRegistry.SkippedDocs, _documentsRegistry.SourceDocs);

            CompareDocuments();

            RemoveDocsFromList(_documents1CDO.SourceDocs, _documents1CDO.MatchedDocs);
            RemoveDocsFromList(_documentsRegistry.SourceDocs, _documentsRegistry.MatchedDocs);
        }
        private void ClearSrcBySkip(List<Document> skipList, List<Document> srcList)
        {
            foreach (Document doc in skipList)
            {
                RemoveSingleDocFromList(srcList, doc);
            }
        }

        private void CompareDocuments()
        {
            _documents1CDO.SourceDocs.ForEach(FindDocumentAddToMatchedSetUPD);

            // Matching related Upp documents
            foreach (Document doc1CUPP in _matchedDocs1CUPPbuffer)
            {
                FindUPDDocumentIn1CUPPAddToMatchList(doc1CUPP);
            }
        }

        // FIrst step of matching documents
        private void FindDocumentAddToMatchedSetUPD(Document doc1CDO)
        {
            _documentsRegistry.SourceDocs.ForEach(delegate(Document docReg)
            {
                bool isMatched = CompareSingleDocumentsAllFields(docReg, doc1CDO);

                if (isMatched)
                {
                    docReg.IsUpd = doc1CDO.IsUpd;

                    if (!_documents1CDO.MatchedDocs.Contains(doc1CDO))
                         _documents1CDO.MatchedDocs.Add(doc1CDO);

                    if(!_documentsRegistry.MatchedDocs.Contains(docReg))
                         _documentsRegistry.MatchedDocs.Add(docReg);

                    if (!_matchedDocs1CUPPbuffer.Contains(docReg))
                         _matchedDocs1CUPPbuffer.Add(docReg);
                }
            });
        }


        // Due to peculiarities in the "1C:UPP" configuration
        // UDP was entered as an invoice + bill of lading or an act of work performed
        // thus I find the second document from the PPM, which is included in the UPD
        private void FindUPDDocumentIn1CUPPAddToMatchList(Document docRegBuff)
        {
            foreach (Document docReg in _documentsRegistry.SourceDocs)
            {
                bool isMatch = false;

                if (docRegBuff != docReg && docRegBuff.IsUpd )
                    isMatch = CompareSingleDocumentsMainFields(docReg, docRegBuff);

                if (isMatch)
                {
                    docReg.IsUpd = true;

                    if (!_documentsRegistry.MatchedDocs.Contains(docReg))
                        _documentsRegistry.MatchedDocs.Add(docReg);
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
            RemoveDocsFromList(docList, docRemoveList);
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
            bool isMainFieldsMatch = CompareSingleDocumentsMainFields(docFirst, docsecond);

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
