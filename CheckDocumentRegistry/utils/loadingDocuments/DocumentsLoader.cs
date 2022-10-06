
namespace CheckDocumentRegistry
{
    public class DocumentsLoader
    {
        
        public List<Document> GetDoDocuments(string doSpreadSheetPath, string exceptedDoPath)
        {
            int[] docFieldIndex1CDo = new int[] { 0, 3, 6, 7, 8, 9, 10, 11 };
            int[] customDocFieldIndex1CDo = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };

            string[][] doDocumentsData = GetDocumentsFromReader(doSpreadSheetPath);

            DocumentsConverter<Document1CDo> newDocumentsConverter = new( docFieldIndex1CDo,
                                                                               customDocFieldIndex1CDo, 
                                                                         8, 12);
            List<Document> documents = newDocumentsConverter.ConvertDocuments(doDocumentsData, exceptedDoPath);


            return documents;
        }


        public List<Document> GetUppDocuments(string uppSpreadSheetPath, string exceptedDoPath)
        {

            int[] docFieldIndex1CUpp = new int[] { 0, 1, 3, 4, 5, 6, 7 };
            int[] customDocFieldIndex1CUpp = new int[] { 0, 1, 2, 3, 4, 5, 6 };

            string[][] uppDocumentsData = GetDocumentsFromReader(uppSpreadSheetPath);

            DocumentsConverter<Document1CUpp> newDocumentsConverter = new(  docFieldIndex1CUpp,
                                                                                 customDocFieldIndex1CUpp,
                                                                           1, 8);
            List<Document> documents = newDocumentsConverter.ConvertDocuments(uppDocumentsData, exceptedDoPath);

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
                    DocumentsConverter<Document> documentsConverter = new();

                    //DocumentsConverter? documentsConverter = new DocumentsConverter();
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
