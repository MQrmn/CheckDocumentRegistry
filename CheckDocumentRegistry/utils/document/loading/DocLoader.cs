using DocumentFormat.OpenXml.Wordprocessing;

namespace RegComparator
{
    public class DocLoader
    {
        private DocFieldsBase _docFieldsSettings;
        private List<Document> _documents;
        

        public DocLoader() { }
        public DocLoader(DocFieldsBase docFields, List<Document> documents)
        {
            _docFieldsSettings = docFields;
            _documents = documents;
        }

        DocFieldsSettings docFieldsSettings = new();

        public void GetDocObjectList<T>(string spreadsheetPath, string passDocsPath) where T: Document
        {
            string[][] docArrs = GetDocsFromFile(spreadsheetPath);

            DocConverter<T> docsConverter = new(_docFieldsSettings.DocFielsdIndex,
                                                _docFieldsSettings.MaxPassedRows,
                                                _docFieldsSettings.RowLenght,
                                                _documents);
            docsConverter.ConvertSpecificDocs(docArrs, passDocsPath);
        }

        public void GetDocObjectList<T>(string[] spreadsheetPathArr, string exceptedDocsPath) where T : Document
        {
            string[][] docArrsTmp;
            DocConverter<T> docsConverter = new(_docFieldsSettings.DocFielsdIndex,
                                    _docFieldsSettings.MaxPassedRows,
                                    _docFieldsSettings.RowLenght,
                                    _documents);

            foreach (var spreadsheetPath in spreadsheetPathArr)
            {
                docArrsTmp = GetDocsFromFile(spreadsheetPath);
                docsConverter.ConvertSpecificDocs(docArrsTmp, exceptedDocsPath);
            }
        }

        // Getting pass-through documents during comparison
        public List<Document> GetDocsPass(string spreadsheetPath)
        {
            List<Document> docObjs = new List<Document>();

            try
            {
                if (File.Exists(spreadsheetPath))
                {
                    string[][] ignore = GetDocsFromFile(spreadsheetPath);
                    DocConverter<Document> docsConverter = new(this.docFieldsSettings.docFieldsIndexCommon);
                    docObjs = docsConverter.ConvertUniversalDocs(ignore);
                }
                else
                    Console.WriteLine($"Файл не найден: {spreadsheetPath}. Список игнорируемых документов не будет заполнен.");
            }
            catch
            {
                Console.WriteLine($"Не удалось прочитать файл: {spreadsheetPath}. Список игнорируемых документов не будет заполнен.");
            }
            return docObjs;
        }

        private string[][] GetDocsFromFile(string spreadsheetPath)
        {
            Console.WriteLine($"Чтение электронной таблицы: {spreadsheetPath}");
            SpreadSheetReaderXLSX spreadSheetReaderXLSX = new SpreadSheetReaderXLSX();
            return spreadSheetReaderXLSX.GetDocumentsFromTable(spreadsheetPath);
        }
    }
}
