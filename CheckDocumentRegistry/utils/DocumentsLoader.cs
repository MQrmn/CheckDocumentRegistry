
namespace CheckDocumentRegistry
{
    public class DocumentsLoader
    {
        SpreadSheetReaderXLSX spreadSheetRepository = new SpreadSheetReaderXLSX();
        DocumentsConverter documentsConverter = new DocumentsConverter();

        public List<Document> GetDoDocuments(ProgramParameters programParameters)
        {
            Console.WriteLine($"Чтение элетронной таблицы: {programParameters.doSpreadSheetPath}");
            string[][] doDocumentsData = this.spreadSheetRepository.GetDocumentsFromTable(programParameters.doSpreadSheetPath);

            Console.WriteLine("Конвертация документов 1С:ДО");
            List<Document> documents = documentsConverter.Convert1CDoDocuments(doDocumentsData, programParameters.exceptedDoPath);

            return documents;
        }

        public List<Document> GetUppDocuments(ProgramParameters programParameters)
        {
            Console.WriteLine($"Чтение элетронной таблицы: {programParameters.uppSpreadSheetPath}");
            string[][] uppDocumentsData = this.spreadSheetRepository.GetDocumentsFromTable(programParameters.uppSpreadSheetPath);

            Console.WriteLine("Конвертация документов 1С:УПП");
            List<Document> documents = documentsConverter.Convert1CUppDocuments(uppDocumentsData, programParameters.exceptedUppPath);

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
