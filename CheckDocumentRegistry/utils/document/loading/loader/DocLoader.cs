namespace RegComparator
{
    public class DocLoader
    {
        private IArrToObjConverter _arrToObjConverter;
        

        public DocLoader() { }
        public DocLoader( IArrToObjConverter arrToObjConverter)
        {
            _arrToObjConverter = arrToObjConverter;
        }

        public void GetDocObjectList<T>(string[] spreadsheetPathArr, string? exceptedDocsPath = null) where T : Document
        {
            string[][] docArrsTmp;
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
