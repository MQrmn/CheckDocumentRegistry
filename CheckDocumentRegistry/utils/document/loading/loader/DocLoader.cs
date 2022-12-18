namespace RegComparator
{
    public class DocLoader
    {
        private IArrToObjConverter _arrToObjConverter;
        private ISpreadSheetReader _spreadSheetReader;
        private DocRepositoryBase _docRepository;
        public event EventHandler<string>? Notify;
          
        public DocLoader( IArrToObjConverter arrToObjConverter, ISpreadSheetReader spreadSheetReader)
        {
            _arrToObjConverter = arrToObjConverter;
            _spreadSheetReader = spreadSheetReader;
        }

        public void FillReposotory()
        {

        }

        public void GetDocObjectList<T>(string[] spreadsheetPathArr, string? exceptedDocsPath = null) where T : Document
        {
            string[][] docArrs;
            foreach (var spreadsheetPath in spreadsheetPathArr)
            {
                docArrs = GetDocsArraysFromFile(spreadsheetPath);
                _arrToObjConverter.ConvertArrToObjs(docArrs, exceptedDocsPath);
            }
        }

        private string[][] GetDocsArraysFromFile(string spreadsheetPath)
        {
            Notify?.Invoke(this, $"Чтение электронной таблицы: {spreadsheetPath}");
            return _spreadSheetReader.GetDocumentsFromTable(spreadsheetPath);
        }
    }
}
