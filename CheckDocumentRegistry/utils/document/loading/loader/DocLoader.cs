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

        public void GetDocObjectList<T>(string[] spreadsheetPathArr, string? exceptedDocsPath = null) where T : Document
        {
            string[][] docArrsTmp;
            DocConverter<T> docsConverter = new(_docFieldsSettings.DocFielsdIndex,
                                                _docFieldsSettings.MaxPassedRows,
                                                _docFieldsSettings.RowLenght,
                                                _documents);

            foreach (var spreadsheetPath in spreadsheetPathArr)
            {
                docArrsTmp = GetDocsFromFile(spreadsheetPath);
                docsConverter.ConvertArrToObjs(docArrsTmp, exceptedDocsPath);
            }
        }

        private string[][] GetDocsFromFile(string spreadsheetPath)
        {
            Console.WriteLine($"Чтение электронной таблицы: {spreadsheetPath}");
            SpreadSheetReaderXLSX spreadSheetReaderXLSX = new SpreadSheetReaderXLSX();
            return spreadSheetReaderXLSX.GetDocumentsFromTable(spreadsheetPath);
        }
    }
}
