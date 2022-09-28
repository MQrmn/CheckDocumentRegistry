
namespace CheckDocumentRegistry
{
    internal class DocumentsConverter
    {
        public static List<Document> Convert1CDoDocumentsStandard(string[][] input)
        {
            List<Document1CDoStandard> doDocuments = new List<Document1CDoStandard>(input.Length);
            int numberOfException = new();
            for (int i = 0; i < input.Length; i++)
            {
                try
                {
                    doDocuments.Add(new Document1CDoStandard(input[i]));
                    numberOfException = 0;
                }
                catch 
                {
                    numberOfException++;
                    if (numberOfException > 7) throw;
                }
            }

            List<Document> documents = doDocuments
                .ConvertAll(new Converter<Document1CDoStandard, Document>(delegate (Document1CDoStandard document) {
                    return (Document)document;
                }));

            return documents;
        }

        public static List<Document> Convert1CDoDocumentsClean(string[][] input)
        {
            List<Document1CDoClean> doDocuments = new List<Document1CDoClean>(input.Length);

            int numberOfException = new();
            for (int i = 0; i < input.Length; i++)
            {
                try
                {
                    doDocuments.Add(new Document1CDoClean(input[i]));
                    numberOfException = 0;
                }
                catch 
                {
                    numberOfException++;
                    if (numberOfException > 1) throw;
                }
            }

            List<Document> documents = doDocuments
                .ConvertAll(new Converter<Document1CDoClean, Document>(delegate (Document1CDoClean document) {
                    return (Document)document;
                }));

            return documents;
        }

        public static List<Document> Convert1CUppDocumentsStandard(string[][] input)
        {
            List<Document1CUppStandard> doDocuments = new List<Document1CUppStandard>(input.Length);
            int numberOfException = new();
            for (int i = 0; i < input.Length; i++)
            {
                try
                {
                    doDocuments.Add(new Document1CUppStandard(input[i]));
                }
                catch
                {
                    numberOfException++;
                    if (numberOfException > 1) throw;
                }
            }

            List<Document> documents = doDocuments
                .ConvertAll(new Converter<Document1CUppStandard, Document>(delegate (Document1CUppStandard document) {
                    return (Document)document;
                }));

            return documents;
        }

        public static List<Document> Convert1CUppDocumentsClean(string[][] input)
        {
            List<Document1CUppClean> doDocuments = new List<Document1CUppClean>(input.Length);
            int numberOfException = new();
            for (int i = 0; i < input.Length; i++)
            {
                try
                {
                    doDocuments.Add(new Document1CUppClean(input[i]));
                }
                catch
                {
                    numberOfException++;
                    if (numberOfException > 1) throw;
                }
            }

            List<Document> documents = doDocuments
                .ConvertAll(new Converter<Document1CUppClean, Document>(delegate (Document1CUppClean document) {
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