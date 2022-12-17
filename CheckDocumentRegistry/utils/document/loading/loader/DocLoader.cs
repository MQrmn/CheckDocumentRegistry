namespace RegComparator
{
    public class DocLoader
    {
        private IArrToObjConverter _arrToObjConverter;
        private ISpreadSheetReader _spreadSheetReader;
        public event EventHandler<string>? Notify;

        public DocLoader( IArrToObjConverter arrToObjConverter, ISpreadSheetReader spreadSheetReader)
        {
            _arrToObjConverter = arrToObjConverter;
            _spreadSheetReader = spreadSheetReader;
        }

        public void GetDocObjectList<T>(string[] spreadsheetPathArr, string? exceptedDocsPath = null) where T : Document
        {
            string[][] docArrsTmp;
            foreach (var spreadsheetPath in spreadsheetPathArr)
            {
                docArrsTmp = GetDocsArraysFromFile(spreadsheetPath);
                _arrToObjConverter.ConvertArrToObjs(docArrsTmp, exceptedDocsPath);
            }
        }

        private string[][] GetDocsArraysFromFile(string spreadsheetPath)
        {
            Notify?.Invoke(this, $"Чтение электронной таблицы: {spreadsheetPath}");
            return _spreadSheetReader.GetDocumentsFromTable(spreadsheetPath);
        }
    }
}
