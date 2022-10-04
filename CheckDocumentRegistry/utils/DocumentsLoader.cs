
namespace CheckDocumentRegistry
{
    public class DocumentsLoader
    {

        public List<Document> GetDoDocuments(ChangeableParameters programParameters)
        {
            SpreadSheetReaderXLSX spreadSheetReaderXLSX = new SpreadSheetReaderXLSX();
            Console.WriteLine($"Чтение элетронной таблицы: {programParameters.doSpreadSheetPath}");
            string[][] doDocumentsData = spreadSheetReaderXLSX.GetDocumentsFromTable(programParameters.doSpreadSheetPath);

            DocumentsConverter? documentsConverter = new DocumentsConverter();
            Console.WriteLine("Конвертация документов 1С:ДО");
            List<Document> documents = documentsConverter.Convert1CDoDocuments(doDocumentsData, programParameters.exceptedDoPath);

            return documents;
        }

        public List<Document> GetUppDocuments(ChangeableParameters programParameters)
        {
            SpreadSheetReaderXLSX spreadSheetReaderXLSX = new SpreadSheetReaderXLSX();
            
            Console.WriteLine($"Чтение элетронной таблицы: {programParameters.uppSpreadSheetPath}");
            string[][] uppDocumentsData = spreadSheetReaderXLSX.GetDocumentsFromTable(programParameters.uppSpreadSheetPath);

            DocumentsConverter? documentsConverter = new DocumentsConverter();
            Console.WriteLine("Конвертация документов 1С:УПП");
            List<Document> documents = documentsConverter.Convert1CUppDocuments(uppDocumentsData, programParameters.exceptedUppPath);

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
    }
}
