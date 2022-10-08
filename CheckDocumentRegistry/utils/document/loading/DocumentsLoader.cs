
namespace CheckDocumentRegistry
{
    internal class DocumentsLoader
    {

        DocumentFieldsIndex documentFieldsIndex = new();

        internal List<Document> GetDocuments1CDO(string spreadsheetPath, string exceptedDocsPath)
        {
            string[][] doDocumentsArrs = GetDocumentsFromWorker(spreadsheetPath);

            DocumentConverter<Document1CDO> documentsConverter = new(this.documentFieldsIndex.DocFieldIndex1CDo,
                                                                           this.documentFieldsIndex.DustomDocFieldIndex1CDo,
                                                                     this.documentFieldsIndex.maxPassedRowForSwitchDo,
                                                                                  this.documentFieldsIndex.rowLenghtDo);
            List<Document> documentsObjs = documentsConverter.ConvertSpecificDocs(doDocumentsArrs, exceptedDocsPath);
            return documentsObjs;
        }


        internal List<Document> GetDocuments1CUPP(string spreadsheetPath, string exceptedDocsPath)
        {
            string[][] documentsArrs = GetDocumentsFromWorker(spreadsheetPath);

            DocumentConverter<Document1CUPP> documentsConverter = new(this.documentFieldsIndex.DocFieldIndex1CUpp,
                                                                            this.documentFieldsIndex.DustomDocFieldIndex1CUpp,
                                                                      this.documentFieldsIndex.maxPassedRowForSwitchUpp,
                                                                                   this.documentFieldsIndex.rowLenghtUpp);
            List<Document> documentsObjs = documentsConverter.ConvertSpecificDocs(documentsArrs, exceptedDocsPath);

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
                    DocumentConverter<Document> documentsConverter = new(this.documentFieldsIndex.docFieldsIndexCommon);
                    documentsObjs = documentsConverter.ConvertUniversalDocs(ignore);
                }
                else
                    Console.WriteLine($"Файл не найден: {spreadsheetPath}. Список игнорируемых документов не будет заполнен.");
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
