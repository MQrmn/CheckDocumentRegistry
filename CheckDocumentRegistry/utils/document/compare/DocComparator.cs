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
        }

        public void CompareDocuments()
        {
            //var i = 1;
            //Console.WriteLine($"{i++} " + DateTime.Now);
            foreach (var doc in _documents1CDO.SourceDocs)
            {
                if (CheckByContaining(_documents1CDO.SkippedDocs, doc))
                    doc.Skip = true;
            }

            //Console.WriteLine($"{i++} " + DateTime.Now);
            foreach (var doc in _documentsRegistry.SourceDocs)
            {
                if (CheckByContaining(_documentsRegistry.SkippedDocs, doc))
                    doc.Skip = true;
            }

            //Console.WriteLine($"{i++} " + DateTime.Now);
            foreach (var doc in _documents1CDO.SourceDocs)
            {
                FillMatchList(doc);
            }
            //Console.WriteLine($"{i++} " + DateTime.Now);
            foreach (var doc in _matchedDocs1CUPPbuffer)
            {
                FindUPD(doc);
            }
            //Console.WriteLine($"{i++} " + DateTime.Now);

            foreach (var doc in _documents1CDO.MatchedDocs)
            {
                FillUnMatchList(_documents1CDO.SourceDocs, _documents1CDO.UnmatchedDocs, doc);
            }

            foreach (var doc in _documentsRegistry.MatchedDocs)
            {
                FillUnMatchList(_documentsRegistry.SourceDocs, _documentsRegistry.UnmatchedDocs, doc);
            }
        }

        // FIrst step of matching documents
        private void FillMatchList(Document doc1CDO)
        {
            foreach (Document docReg in _documentsRegistry.SourceDocs)
            {
                bool isMatched = CompareByAllFields(docReg, doc1CDO) && 
                                    !doc1CDO.Skip && 
                                    !docReg.Skip;
                if (isMatched)
                {
                    docReg.IsUpd = doc1CDO.IsUpd;

                    if (!_documents1CDO.MatchedDocs.Contains(doc1CDO))
                        _documents1CDO.MatchedDocs.Add(doc1CDO);
                    
                    if (!_matchedDocs1CUPPbuffer.Contains(docReg))
                        _matchedDocs1CUPPbuffer.Add(docReg);

                    if (!_documentsRegistry.MatchedDocs.Contains(docReg))
                        _documentsRegistry.MatchedDocs.Add(docReg);
                }
            }
        }

        private void FillUnMatchList(List<Document> docListSrc, List<Document> docListUnmatch, Document docMatch)
        {
            if (!docListSrc.Contains(docMatch))
            {
                if (!docListUnmatch.Contains(docMatch) && !docMatch.Skip)
                {
                    docListUnmatch.Add(docMatch);
                }
            }
        }

        private bool CheckByContaining(List<Document> docList, Document document)
        {
            bool isContainTmp;
            bool isContainRes = false;
            foreach (var doc in docList)
            {
                isContainTmp = CompareByAllFields(document, doc);
                isContainRes = isContainRes ? isContainTmp : isContainRes;
                if (isContainRes)
                    return isContainRes;
            }
            return isContainRes;
        }

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
        private bool CompareByAllFields(Document docFirst, Document docsecond)
        {
            bool isMainFieldsMatch = CompareByMainFields(docFirst, docsecond);

            if (!isMainFieldsMatch) return isMainFieldsMatch;
            else return docFirst.Type == docsecond.Type;
        }

        private bool CompareByMainFields(Document docFirst, Document docsecond)
        {
            return docFirst.Number == docsecond.Number
                            && docFirst.Salary == docsecond.Salary
                            && docFirst.Date == docsecond.Date;
        }
    }
}
