namespace RegistryComparator
{
    internal interface IDocumentsRepository
    {
        public List<Document> Src1CDO;                          // Source documents in 1C:Document Management
        public List<Document> SrcRegistry;                      // Source documents in 1C:UPP
        public List<Document> Matched1CDO;                      // Documents from 1C:Document Management that coincided with 1C:UPP documents
        public List<Document> MatchedRegistry;                  // Documents from 1C:UPP that coincided with 1C:Document Management documents
        public List<Document> Unmatched1CDO;                    // Documents from 1C:Document Management that not coincided with 1C:UPP documents
        public List<Document> UnmatchedRegistry;                // Documents from 1C:UPP that not coincided with 1C:Document Management documents
        public List<Document> Pass1CDO;                         // Ignored documents in 1C:Document Management
        public List<Document> PassRegistry;                     // Ignored documents in 1C:UPP
        public List<Document> MatchedRegistryBuffer;
    }
}
