namespace RegComparator
{
    internal class DocRepositoryRegistry : DocRepositoryBase
    {
        public override void AddSourceDoc(string[] docFieldsArr, int[] docFieldsIndex)
        {
            SourceDocs.Add(new Document1CDO(docFieldsArr, docFieldsIndex));
        }
    }
}
