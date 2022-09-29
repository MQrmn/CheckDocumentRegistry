
namespace CheckDocumentRegistry
{
    internal class DocumentsConverter
    {
        private static int[] docFieldIndex1CDo = new int[] { 0, 3, 6, 7, 8, 9, 10, 11 };        // Standard 1C:DO document format
        private static int[] customDocFieldIndex1CDo = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };    // Simplified 1C:DO document format
        private static int[] docFieldIndex1CUpp = new int[] { 0, 1, 3, 4, 5, 6, 7 };            // Standard 1C:UPP document format
        private static int[] customDocFieldIndex1CUpp = new int[] { 0, 1, 2, 3, 4, 5, 6 };      // Simplified 1C:UPP document format

        public static List<Document> Convert1CDoDocuments(string[][] input)
        {
            int[] fieldIndex = docFieldIndex1CDo;
            List<Document1CDo> doDocuments = new List<Document1CDo>(input.Length);

            int numberOfException = new();
            for (int i = 0; i < input.Length; i++)
            {
                try
                {
                    doDocuments.Add(new Document1CDo(input[i], fieldIndex));
                    numberOfException = 0;
                }
                catch 
                {
                    numberOfException++;
                    if (numberOfException > 8) throw;
                    if (numberOfException > 7) fieldIndex = customDocFieldIndex1CDo;
                }
            }

            List<Document> documents = doDocuments
                .ConvertAll(new Converter<Document1CDo, Document>(delegate (Document1CDo document) {
                    return (Document)document;
                }));

            return documents;
        }

        public static List<Document> Convert1CUppDocuments(string[][] input)
        {
            int[] fieldIndex = docFieldIndex1CUpp;
            List<Document1CUpp> doDocuments = new List<Document1CUpp>(input.Length);
            int numberOfException = new();
            for (int i = 0; i < input.Length; i++)
            {
                try
                {
                    doDocuments.Add(new Document1CUpp(input[i], fieldIndex));
                }
                catch
                {
                    numberOfException++;
                    if (numberOfException > 2) throw;
                    if (numberOfException > 1) fieldIndex = customDocFieldIndex1CUpp;
                }
            }

            List<Document> documents = doDocuments
                .ConvertAll(new Converter<Document1CUpp, Document>(delegate (Document1CUpp document) {
                    return (Document)document;
                }));

            return documents;
        }

        public static List<Document> ConvertIgnoreDoc(string[][] input)
        {

            List<Document> documents = new List<Document>(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                try
                {
                    documents.Add(new Document(input[i]));
                }
                catch { }
            }
            return documents;
        }
    }
}