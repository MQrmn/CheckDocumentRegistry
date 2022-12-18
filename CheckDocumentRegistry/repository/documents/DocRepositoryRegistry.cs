namespace RegComparator
{
    public class DocRepositoryRegistry : DocRepositoryBase
    {
        private byte _workMode;
        public DocRepositoryRegistry(byte workMode = 1) 
        {
            _workMode = workMode;
        }
        public override void AddSourceDoc(string[] docFieldsArr, int[] docFieldsIndex)
        {
            if (_workMode == 1)
                SourceDocs.Add(new Document1CKA(docFieldsArr, docFieldsIndex));
            else
                SourceDocs.Add(new Document1CUPP(docFieldsArr, docFieldsIndex));
        }
    }
}
