
namespace CheckDocumentRegistry
{
    public class DocumentsLoader
    {
        SpreadSheetReaderXLSX spreadSheetRepository = new SpreadSheetReaderXLSX();

        public List<Document> GetDoDocuments(string filePath)
        {
            Console.WriteLine($"Чтение элетронной таблицы: {filePath}");
            string[][] doDocumentsData = this.spreadSheetRepository.GetDocumentsFromTable(filePath);

            Console.WriteLine("Конвертация документов 1С:ДО");
            List<Document> documents = DocumentsConverter.Convert1CDoDocuments(doDocumentsData);

            return documents;
        }

        public List<Document> GetUppDocuments(string filePath)
        {
            Console.WriteLine($"Чтение элетронной таблицы: {filePath}");
            string[][] uppDocumentsData = this.spreadSheetRepository.GetDocumentsFromTable(filePath);

            Console.WriteLine("Конвертация документов 1С:УПП");
            List<Document> documents = DocumentsConverter.Convert1CUppDocuments(uppDocumentsData);

            return documents;
        }

        public List<Document> GetIgnore(string filePath)
        {
            
            List<Document> documents = new List<Document>();

            try
            {
                if (File.Exists(filePath))
                {
                    Console.WriteLine($"Чтение элетронной таблицы: {filePath}");
                    string[][] ignore = this.spreadSheetRepository.GetDocumentsFromTable(filePath);
                    documents = DocumentsConverter.ConvertIgnoreDoc(ignore);
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
    }
}
