
namespace CheckDocumentRegistry
{
    internal class Converter
    {
        public static List<Document> ConvertDoDocuments(string[][] input)
        {
            List<DoDocument> doDocuments = new List<DoDocument>(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                doDocuments.Add(new DoDocument(input[i]));
            }

            List<Document> documents = doDocuments
                .ConvertAll(new Converter<DoDocument, Document>(delegate (DoDocument document) {
                    return (Document)document;
                }));

            return documents;
        }

        public static List<Document> ConvertUppDocuments(string[][] input)
        {
            List<UppDocument> doDocuments = new List<UppDocument>(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                doDocuments.Add(new UppDocument(input[i]));
            }

            List<Document> documents = doDocuments
                .ConvertAll(new Converter<UppDocument, Document>(delegate (UppDocument document) {
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