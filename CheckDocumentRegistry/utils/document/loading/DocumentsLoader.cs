
namespace CheckDocumentRegistry
{
    internal class DocumentsLoader
    {

        DocumentFieldsIndex documentFieldsIndex = new();

        internal List<Document> GetDocuments1CDO(string spreadsheetPath, string exceptedDocsPath)
        {
            string[][] doDocumentsArrs = GetDocumentsFromWorker(spreadsheetPath);

            DocumentsConverter<Document1CDO> documentsConverter = new(this.documentFieldsIndex.DocFieldIndex1CDo,
                                                                           this.documentFieldsIndex.DustomDocFieldIndex1CDo,
                                                                     this.documentFieldsIndex.maxPassedRowForSwitchDo,
                                                                                  this.documentFieldsIndex.rowLenghtDo);
            List<Document> documentsObjs = documentsConverter.ConvertDocuments(doDocumentsArrs, exceptedDocsPath);
            return documentsObjs;
        }


        internal List<Document> GetDocuments1CUPP(string spreadsheetPath, string exceptedDocsPath)
        {
            string[][] documentsArrs = GetDocumentsFromWorker(spreadsheetPath);

            DocumentsConverter<Document1CUPP> documentsConverter = new(this.documentFieldsIndex.DocFieldIndex1CUpp,
                                                                            this.documentFieldsIndex.DustomDocFieldIndex1CUpp,
                                                                      this.documentFieldsIndex.maxPassedRowForSwitchUpp,
                                                                                   this.documentFieldsIndex.rowLenghtUpp);
            List<Document> documentsObjs = documentsConverter.ConvertDocuments(documentsArrs, exceptedDocsPath);

            return documentsObjs;
        }




        internal List<Document> GetIgnoreDocs(string spreadsheetPath)
        {
            List<Document> documentsObjs = new List<Document>();

            try
            {
                if (File.Exists(spreadsheetPath))
                {
                    string[][] ignore = GetDocumentsFromWorker(spreadsheetPath);
                    DocumentsConverter<Document> documentsConverter = new();
                    documentsObjs = documentsConverter.ConvertIgnoreDoc(ignore);
                }
                else
                {
                    Console.WriteLine($"Файл не найден: {spreadsheetPath}. Список игнорируемых документов не будет заполнен.");
                }
            }
            catch
            {
                Console.WriteLine($"Не удалось прочитать файл: {spreadsheetPath}. Список игнорируемых документов не будет заполнен.");
            }
            return documentsObjs;
        }


        private string[][] GetDocumentsFromWorker(string spreadsheetPath)
        {
            Console.WriteLine($"Чтение электронной таблицы: {spreadsheetPath}");
            SpreadSheetReaderXLSX spreadSheetReaderXLSX = new SpreadSheetReaderXLSX();
            return spreadSheetReaderXLSX.GetDocumentsFromTable(spreadsheetPath);
        }
    }
}
