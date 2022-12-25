namespace RegComparator
{
    internal class DocComparator_new : IDocComparator
    {
        private List<Document> _matchedDocs1CUPPbuffer = new();
        private DocRepositoryBase _documents1CDO;
        private DocRepositoryBase _documentsRegistry;

        public DocComparator_new(DocRepositoryBase documents1CDO, DocRepositoryBase documentsRegistry)
        {
            _documents1CDO = documents1CDO;
            _documentsRegistry = documentsRegistry;




        }





    }
}
