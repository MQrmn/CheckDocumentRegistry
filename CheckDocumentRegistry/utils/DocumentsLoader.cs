
namespace CheckDocumentRegistry
{
    public class DocumentsLoader
    {


        public List<Document> GetDoDocuments(string doSpreadSheetPath, string exceptedDoPath)
        {
            string[][] doDocumentsData = GetDocumentsFromReader(doSpreadSheetPath);
            Console.WriteLine("Конвертация документов 1С:ДО");
            DocumentsConverter? documentsConverter = new DocumentsConverter();
            List<Document> documents = documentsConverter.Convert1CDoDocuments(doDocumentsData, exceptedDoPath);

            return documents;
        }

        


        public List<Document> GetUppDocuments(string uppSpreadSheetPath, string exceptedDoPath)
        {
            
            string[][] uppDocumentsData = GetDocumentsFromReader(uppSpreadSheetPath);
            Console.WriteLine("Конвертация документов 1С:УПП");
            DocumentsConverter? documentsConverter = new DocumentsConverter();
            List<Document> documents = documentsConverter.Convert1CUppDocuments(
                                                                    uppDocumentsData,
                                                                    exceptedDoPath);

            return documents;
        }


        public List<Document> GetIgnore(string filePath)
        {
            SpreadSheetReaderXLSX spreadSheetReaderXLSX = new SpreadSheetReaderXLSX();
            List<Document> documents = new List<Document>();

            try
            {
                if (File.Exists(filePath))
                {
                    Console.WriteLine($"Чтение элетронной таблицы: {filePath}");
                    string[][] ignore = spreadSheetReaderXLSX.GetDocumentsFromTable(filePath);
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
            Console.WriteLine($"Чтение элетронной таблицы: {doSpreadSheetPath}");
            SpreadSheetReaderXLSX spreadSheetReaderXLSX = new SpreadSheetReaderXLSX();
            return spreadSheetReaderXLSX.GetDocumentsFromTable(doSpreadSheetPath);

        }
    }
}
