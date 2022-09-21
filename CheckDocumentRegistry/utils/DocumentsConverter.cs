
namespace CheckDocumentRegistry
{
    internal class DocumentsConverter
    {
        public static List<Document> ConvertDoDocuments(string[][] input)
        {
            List<Document1CDO> doDocuments = new List<Document1CDO>(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                doDocuments.Add(new Document1CDO(input[i]));
            }

            List<Document> documents = doDocuments
                .ConvertAll(new Converter<Document1CDO, Document>(delegate (Document1CDO document) {
                    return (Document)document;
                }));

            return documents;
        }

        public static List<Document> ConvertUppDocuments(string[][] input)
        {
            List<Document1CUpp> doDocuments = new List<Document1CUpp>(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                doDocuments.Add(new Document1CUpp(input[i]));
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
                documents.Add(new Document(input[i]));
            }

            return documents;

        }
    }
}