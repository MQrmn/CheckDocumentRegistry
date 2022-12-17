namespace RegComparator
{
    internal class DocRepository1CDO : DocRepositoryBase
    {
        public override void AddSourceDoc(string[] docFieldsArr, int[] docFieldsIndex)
        {
            SourceDocs.Add(new Document1CDO(docFieldsArr, docFieldsIndex));
        }
    }
}
