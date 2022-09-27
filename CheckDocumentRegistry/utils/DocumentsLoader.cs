
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
            List<Document> documents = DocumentsConverter.ConvertDoDocuments(doDocumentsData);

            return documents;
        }

        public List<Document> GetUppDocuments(string filePath)
        {
            Console.WriteLine($"Reading Table {filePath}");
            string[][] uppDocumentsData = this.spreadSheetRepository.GetDocumentsFromTable(filePath);

            Console.WriteLine("Converting Upp data");
            List<Document> documents = DocumentsConverter.ConvertUppDocuments(uppDocumentsData);

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
