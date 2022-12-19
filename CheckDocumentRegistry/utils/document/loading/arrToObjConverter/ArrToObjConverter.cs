namespace RegComparator
{

    internal class ArrToObjConverter : IArrToObjConverter
    {
        private List<string[]>? _exceptedDocs;
        public event EventHandler<string>? ErrNotify;

        // Specific documents is exported from 1C:DO or 1C:UPP spreadsheets
        public void ConvertArrToObjs(   string[][] docsArr,
                                        Action<string[], int[]> addDocument,
                                        DocFieldsBase fieldsSettings
                                        )
        {
            int exceptCount = 0;
            // Going through an array of documents
            for (int i = 0; i < docsArr.Length; i++)                    
            {
                try
                {
                    // Adding document to repository
                    addDocument(docsArr[i], fieldsSettings.DocFielsdIndex);
                }
                catch
                {
                    exceptCount++;
                    if (exceptCount > fieldsSettings.MaxPassedRows)
                        // Adding document to error list 
                        _exceptedDocs?.Add(docsArr[i]);                                                       
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
                    ErrNotify?.Invoke(this, docField + " ");    // Using Console.WriteLine unsted Console.Write
                }
                rowCount++;
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}