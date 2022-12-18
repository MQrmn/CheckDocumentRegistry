namespace RegComparator
{

    internal class ArrToObjConverter<T> : IArrToObjConverter where T : Document
    {
        private List<string[]>? _exceptedDocs;
        private List<Document> _docObjects;
        private DocFieldsBase _fieldsSettings;

        //public delegate void ArrToObjConverterEvents(string message);
        public event EventHandler<string>? ErrNotify;

        internal ArrToObjConverter(DocFieldsBase fieldsSettings,
                                 List<Document> docObjects)
        {
            _fieldsSettings = fieldsSettings;
            _exceptedDocs = new List<string[]>();
            _docObjects = docObjects;
        }

        // Specific documents is exported from 1C:DO or 1C:UPP spreadsheets
        public void ConvertArrToObjs(string[][] docsArr, string? passedDocsReportPath)
        {
            int exceptCount = 0;

            for (int i = 0; i < docsArr.Length; i++)                                                        // Going through an array of documents
            {
                try
                {
                    _docObjects.Add((T)Activator.CreateInstance(typeof(T), docsArr[i], _fieldsSettings.DocFielsdIndex));   // Adding a document object to the list
                }
                catch
                {
                    exceptCount++;
                    if (exceptCount > _fieldsSettings.MaxPassedRows)
                        _exceptedDocs?.Add(docsArr[i]);                                                       // Adding document into error list 
                }
            }

            if (passedDocsReportPath is not null && _exceptedDocs?.Count > 0)
                CreateReportPassedDocs(passedDocsReportPath);
        }

        private void CreateReportPassedDocs(string exceptedUppPath)
        {
            PrintConsolePassedDocs(_exceptedDocs);
            SpreadSheetWriterXLSX spreadSheetWriterXLSX = new SpreadSheetWriterXLSX(exceptedUppPath);
            spreadSheetWriterXLSX.CreateSpreadsheet(_exceptedDocs);
        }

        private void PrintConsolePassedDocs(List<string[]> exceptedDocuments)
        {
            ErrNotify?.Invoke(this, "В следующих строках возникли исключения:");
            int rowCount = 1;
            foreach (var exeptDoc in exceptedDocuments)
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