namespace RegComparator
{
    public class DocLoader
    {
        //private DocFieldsBase _docFieldsSettings;
        //private List<Document> _documents;
        private IArrToObjConverter _arrToObjConverter;
        

        public DocLoader() { }
        public DocLoader( IArrToObjConverter arrToObjConverter)
        {
            //_docFieldsSettings = docFields;
            //_documents = documents;
            _arrToObjConverter = arrToObjConverter;
        }

        public void GetDocObjectList<T>(string[] spreadsheetPathArr, string? exceptedDocsPath = null) where T : Document
        {
            string[][] docArrsTmp;
            //ArrToObjConverter<T> docsConverter = new(_docFieldsSettings.DocFielsdIndex,
            //                                    _docFieldsSettings.MaxPassedRows,
            //                                    _docFieldsSettings.RowLenght,
            //                                    _documents);

            foreach (var spreadsheetPath in spreadsheetPathArr)
            {
                docArrsTmp = GetDocsFromFile(spreadsheetPath);
                _arrToObjConverter.ConvertArrToObjs(docArrsTmp, exceptedDocsPath);
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
