
namespace CheckDocumentRegistry
{
    public class DocumentsLoader
    {
        SpreadSheetReaderXLSX spreadSheetRepository = new SpreadSheetReaderXLSX();


        public List<Document> GetDoDocuments(string filePath)
        {
            Console.WriteLine($"Reading Table {filePath}");
            string[][] doDocumentsData = this.spreadSheetRepository.GetDocumentsFromTable(filePath);

            Console.WriteLine("Converting Do data");

            List<Document> documents;
            try
            {
                documents = DocumentsConverter.Convert1CDoDocumentsStandard(doDocumentsData);
            }
            catch
            {
                documents = DocumentsConverter.Convert1CDoDocumentsClean(doDocumentsData);
            }
            
            return documents;
        }

        public List<Document> GetUppDocuments(string filePath)
        {
            Console.WriteLine($"Reading Table {filePath}");
            string[][] uppDocumentsData = this.spreadSheetRepository.GetDocumentsFromTable(filePath);
            List<Document> documents;
            Console.WriteLine("Converting Upp data");
            try
            {
                documents = DocumentsConverter.Convert1CUppDocumentsStandard(uppDocumentsData);
            }
            catch
            {
                documents = DocumentsConverter.Convert1CUppDocumentsClean(uppDocumentsData);
            }

            return documents;
        }

        public List<Document> GetIgnore(string filePath)
        {
            Console.WriteLine($"Reading Table {filePath}");
            string[][] ignore = this.spreadSheetRepository.GetDocumentsFromTable(filePath);
            List<Document> documents = DocumentsConverter.ConvertIgnoreDoc(ignore);
            return documents;
        }
    }
}
