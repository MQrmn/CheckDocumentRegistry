namespace RegComparator
{

    internal class ArrToObjConverter_new : IArrToObjConverter
    {
        private List<string[]>? _exceptedDocs;
        public event EventHandler<string>? ErrNotify;

        public void ConvertArrToObjs(string[][] docsArr, string? passedDocsReportPath) => 
            throw new NotImplementedException();

        // Specific documents is exported from 1C:DO or 1C:UPP spreadsheets
        public void ConvertArrToObjs(
                                        string[][] docsArr,
                                        Action<string[], int[]> addDocument,
                                        DocFieldsBase fieldsSettings
                                        )
        {
            int exceptCount = 0;

            for (int i = 0; i < docsArr.Length; i++)                                                        // Going through an array of documents
            {
                try
                {
                    addDocument(docsArr[i], fieldsSettings.DocFielsdIndex);
                }
                catch
                {
                    exceptCount++;
                    if (exceptCount > fieldsSettings.MaxPassedRows)
                        _exceptedDocs?.Add(docsArr[i]);                                                       // Adding document into error list 
                }
            }

            if (_exceptedDocs?.Count > 0)
                PrintConsolePassedDocs();
        }

        private void PrintConsolePassedDocs()
        {
            ErrNotify?.Invoke(this, "В следующих строках возникли исключения:");
            int rowCount = 1;
            foreach (var exeptDoc in _exceptedDocs)
            {
                ErrNotify?.Invoke(this, rowCount + ". ");
                foreach (var docField in exeptDoc)
                {
                    ErrNotify?.Invoke(this, docField + " ");
                }
                rowCount++;
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}