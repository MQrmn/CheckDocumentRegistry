
namespace RegComparator
{
    public abstract class DocRepositoryBase
    {
        public List<Document> SourceDocs;
        public List<Document> MatchedDocs;
        public List<Document> UnmatchedDocs;
        public List<Document> SkippedDocs;
        
        public DocRepositoryBase()
        {
            SourceDocs = new();
            MatchedDocs = new();
            UnmatchedDocs = new();
            SkippedDocs = new();
        }

        public abstract void AddSourceDoc(string[] docFieldsArr, int[] docFieldsIndex);
        public virtual void AddSkippedDoc(string[] docFieldsArr, int[] docFieldsIndex) {
            SkippedDocs.Add(new Document(docFieldsArr, docFieldsIndex));
        }

    }
}