namespace RegComparator
{

    internal class ArrToObjConverter<T> : IArrToObjConverter where T : Document
    {
        private List<string[]>? _passedDocs;
        //private int[] _docFieldsIndex;
        //private int _maxPassedRows;
        //private int _rowLenght;
        private List<Document> _docObjects;
        private DocFieldsBase _docFieldsSettings;


        internal ArrToObjConverter(DocFieldsBase docFieldsSettings,
                                 List<Document> docObjects)
        {
            _docFieldsSettings = docFieldsSettings;

            //_docFieldsIndex = docFieldsIndex;
            _passedDocs = new List<string[]>();
            //_maxPassedRows = maxPassedRows;
            //_rowLenght = inputRowLength;
            _docObjects = docObjects;
        }

        //internal ArrToObjConverter(int[] inputDocFileIndex)
        //{
        //    _docFieldsIndex = inputDocFileIndex;
        //}

        // Specific documents is exported from 1C:DO or 1C:UPP spreadsheets
        public void ConvertArrToObjs(string[][] docsArr, string? passedDocsReportPath)
        {
            int exceptCount = 0;

            for (int i = 0; i < docsArr.Length; i++)                                                        // Going through an array of documents
            {
                try
                {
                    _docObjects.Add((T)Activator.CreateInstance(typeof(T), docsArr[i], _docFieldsSettings.DocFielsdIndex));   // Adding a document object to the list
                }
                catch
                {
                    exceptCount++;
                    if (exceptCount > _docFieldsSettings.MaxPassedRows)
                        _passedDocs?.Add(docsArr[i]);                                                       // Adding document into error list 
                }
            }

            if (passedDocsReportPath is not null && _passedDocs?.Count > 0)
                CreateReportPassedDocs(passedDocsReportPath);
        }

        private void CreateReportPassedDocs(string exceptedUppPath)
        {
            PrintConsolePassedDocs(_passedDocs);
            SpreadSheetWriterXLSX spreadSheetWriterXLSX = new SpreadSheetWriterXLSX(exceptedUppPath);
            spreadSheetWriterXLSX.CreateSpreadsheet(_passedDocs);
        }

        private void PrintConsolePassedDocs(List<string[]> exceptedDocuments)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("В следующих строках возникли исключения:");
            int rowCount = 1;
            foreach (var exeptDoc in exceptedDocuments)
            {
                Console.Write(rowCount + ". ");
                foreach (var docField in exeptDoc)
                {
                    Console.Write(docField + " ");
                }
                rowCount++;
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}