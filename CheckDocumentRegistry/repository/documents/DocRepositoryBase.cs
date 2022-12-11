
namespace RegComparator
{
    public class DocRepositoryBase
    {
        public List<DocumentBase> Src1CDO;                          // Source documents in 1C:Document Management
        public List<DocumentBase> SrcRegistry;                      // Source documents in 1C:UPP
        public List<DocumentBase> Matched1CDO;                      // Documents from 1C:Document Management that coincided with 1C:UPP documents
        public List<DocumentBase> MatchedRegistry;                  // Documents from 1C:UPP that coincided with 1C:Document Management documents
        public List<DocumentBase> Unmatched1CDO;                    // Documents from 1C:Document Management that not coincided with 1C:UPP documents
        public List<DocumentBase> UnmatchedRegistry;                // Documents from 1C:UPP that not coincided with 1C:Document Management documents
        public List<DocumentBase> Pass1CDO;                         // Ignored documents in 1C:Document Management
        public List<DocumentBase> PassRegistry;                     // Ignored documents in 1C:UPP
        public List<DocumentBase> MatchedRegistryBuffer;

        public DocRepositoryBase()
        {
            Src1CDO = new();                          
            SrcRegistry = new();                      
            Matched1CDO = new();                       
            MatchedRegistry = new();                 
            Unmatched1CDO = new();                   
            UnmatchedRegistry = new();                 
            Pass1CDO = new();                         
            PassRegistry = new();                     
            MatchedRegistryBuffer = new();

    }
    }
}