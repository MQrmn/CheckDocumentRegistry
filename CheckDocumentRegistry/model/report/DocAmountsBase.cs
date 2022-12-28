namespace RegComparator
{
    public class DocAmountsBase
    {
        public int Source;
        public int Skip;
        public int Matched;
        public int Unmatched;
        private DocRepositoryBase _docRepo;

        public DocAmountsBase(DocRepositoryBase docRepo)
        {
            _docRepo = docRepo;
            Source = _docRepo.SourceDocs.Count;
            Skip = _docRepo.SkippedDocs.Count;
            Matched = _docRepo.MatchedDocs.Count;
            Unmatched = _docRepo.UnmatchedDocs.Count;
    }
    }
}
