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

        public void GetDocObjectList(
                                        string[] spreadsheetPathArr,
                                        Action<string[], int[]> addDocumentAction,
                                        DocFieldsBase fieldsSettings
                                        )
        {
            string[][] docArrsTmp;
            foreach (var spreadsheetPath in spreadsheetPathArr)
            {
                if (spreadsheetPath is not null)
                {
                    docArrsTmp = GetDocsArraysFromFile(spreadsheetPath);
                    _arrToObjConverter.ConvertArrToObjs(docArrsTmp, addDocumentAction, fieldsSettings);
                }
                
            }
        }

        private string[][] GetDocsArraysFromFile(string spreadsheetPath)
        {
            Notify?.Invoke(this, $"Чтение электронной таблицы: {spreadsheetPath}");
            return _spreadSheetReader.GetDocumentsFromTable(spreadsheetPath);
        }
    }
}
