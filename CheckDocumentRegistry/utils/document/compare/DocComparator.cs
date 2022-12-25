namespace RegComparator
{
    internal class DocComparator : IDocComparator
    {
        private List<Document> _matchedRegBuffer;
        private DocRepositoryBase _documents1CDO;
        private DocRepositoryBase _documentsRegistry;

        public DocComparator(DocRepositoryBase documents1CDO, DocRepositoryBase documentsRegistry)
        {
            _documents1CDO = documents1CDO;
            _documentsRegistry = documentsRegistry;
            _matchedRegBuffer = new();
        }

        public void CompareDocuments()
        {
            MarkSkipped();

            _documents1CDO.SourceDocs.ForEach(FillMatchList);
            _matchedRegBuffer.ForEach(FindUPD);

            FillAllUnmatched();
        }

        private void MarkSkipped()
        {
            foreach (var doc in _documents1CDO.SourceDocs)
            {
                if (CheckContainingByFields(_documents1CDO.SkippedDocs, doc))
                    doc.Skip = true;
            }

            foreach (var doc in _documentsRegistry.SourceDocs)
            {
                if (CheckContainingByFields(_documentsRegistry.SkippedDocs, doc))
                    doc.Skip = true;
            }
        }

        private void FillAllUnmatched()
        {
            foreach (var doc in _documents1CDO.SourceDocs)
            {
                FillUnmatched(_documents1CDO.MatchedDocs, _documents1CDO.UnmatchedDocs, doc);
            }

            foreach (var doc in _documentsRegistry.SourceDocs)
            {
                FillUnmatched(_documentsRegistry.MatchedDocs, _documentsRegistry.UnmatchedDocs, doc);
            }
        }

        // FIrst step of matching documents
        private void FillMatchList(Document doc1CDO)
        {
            foreach (Document docReg in _documentsRegistry.SourceDocs)
            {
                bool isMatched = CompareByFields(docReg, doc1CDO);

                if (isMatched)
                {
                    docReg.IsUpd = doc1CDO.IsUpd;

                    if (!_documents1CDO.MatchedDocs.Contains(doc1CDO))
                        _documents1CDO.MatchedDocs.Add(doc1CDO);
                    
                    if (!_matchedRegBuffer.Contains(docReg))
                        _matchedRegBuffer.Add(docReg);

                    if (!_documentsRegistry.MatchedDocs.Contains(docReg))
                        _documentsRegistry.MatchedDocs.Add(docReg);
                }
            }
        }

        private void FillUnmatched(List<Document> docListMatch, List<Document> docListUnmatch, Document docSrc)
        {
            if (!docListMatch.Contains(docSrc))
            {
                if (!docListUnmatch.Contains(docSrc) && !docSrc.Skip)
                {
                    docListUnmatch.Add(docSrc);
                }
            }
        }

        private bool CheckContainingByFields(List<Document> docList, Document document)
        {
            bool isContainTmp;
            bool isContainRes = false;
            foreach (var doc in docList)
            {
                isContainTmp = CompareByFields(document, doc);
                isContainRes = !isContainRes ? isContainTmp : isContainRes;
                if (isContainRes)
                    return isContainRes;
            }
            return isContainRes;
        }

        // Finding UPD documents 
        private void FindUPD(Document docRegBuff)
        {
            foreach (Document docReg in _documentsRegistry.SourceDocs)
            {
                bool isMatch = false;

                if (docRegBuff != docReg && docRegBuff.IsUpd )
                    isMatch = CompareByMainFields(docReg, docRegBuff);

                if (isMatch)
                {
                    docReg.IsUpd = true;

                    if (!_documentsRegistry.MatchedDocs.Contains(docReg))
                        _documentsRegistry.MatchedDocs.Add(docReg);
                }
            }
        }

        // Comparing documents
        private bool CompareByFields(Document docFirst, Document docsecond)
        {
            bool isMainFieldsMatch = CompareByMainFields(docFirst, docsecond);

            if (!isMainFieldsMatch) 
                return isMainFieldsMatch;
            else 
                return docFirst.Type == docsecond.Type;
        }

        private bool CompareByMainFields(Document docFirst, Document docsecond)
        {
            return docFirst.Number == docsecond.Number
                            && docFirst.Salary == docsecond.Salary
                            && docFirst.Date == docsecond.Date;
        }
    }
}
