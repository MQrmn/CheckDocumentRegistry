
namespace CheckDocumentRegistry
{
    public class DocumentsLoader
    {


        public List<Document> GetDoDocuments(string doSpreadSheetPath, string exceptedDoPath)
        {
            string[][] doDocumentsData = GetDocumentsFromReader(doSpreadSheetPath);
            DocumentsConverter? documentsConverter = new DocumentsConverter();
            List<Document> documents = documentsConverter.Convert1CDoDocuments(doDocumentsData, exceptedDoPath);

            return documents;
        }


        public List<Document> GetUppDocuments(string uppSpreadSheetPath, string exceptedDoPath)
        {
            
            string[][] uppDocumentsData = GetDocumentsFromReader(uppSpreadSheetPath);
            DocumentsConverter? documentsConverter = new DocumentsConverter();
            List<Document> documents = documentsConverter.Convert1CUppDocuments(
                                                                    uppDocumentsData,
                                                                    exceptedDoPath);

            return documents;
        }


        public List<Document> GetIgnore(string filePath)
        {
            
            List<Document> documents = new List<Document>();

            try
            {
                if (File.Exists(filePath))
                {
                    string[][] ignore = GetDocumentsFromReader(filePath);
                    DocumentsConverter? documentsConverter = new DocumentsConverter();
                    documents = documentsConverter.ConvertIgnoreDoc(ignore);
                }
                else
                {
                    Console.WriteLine($"Файл не найден: {filePath}. Список игнорируемых документов не будет заполнен.");
                }
            }

            catch
            {
                Console.WriteLine($"Не удалось прочитать файл: {filePath}. Список игнорируемых документов не будет заполнен.");
            }

            return documents;
        }


        private string[][] GetDocumentsFromReader(string doSpreadSheetPath)
        {
            Console.WriteLine($"Чтение электронной таблицы: {doSpreadSheetPath}");
            SpreadSheetReaderXLSX spreadSheetReaderXLSX = new SpreadSheetReaderXLSX();
            return spreadSheetReaderXLSX.GetDocumentsFromTable(doSpreadSheetPath);
        }
    }
}
