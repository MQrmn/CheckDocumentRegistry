namespace RegComparator
{

    internal class DocConverter<T> where T : Document
    {
        internal List<string[]>? _passedDocs;
        private int[] _docFieldsIndex;
        private int _maxPassedRows;
        private int _rowLenght;

        private List<Document> _docObjects;

        internal DocConverter(int[] docFieldsIndex,
                                 int maxPassedRows,
                                 int inputRowLength,
                                 List<Document> docObjects)
        {
            _docFieldsIndex = docFieldsIndex;
            _passedDocs = new List<string[]>();
            _maxPassedRows = maxPassedRows;
            _rowLenght = inputRowLength;
            _docObjects = docObjects;
        }

        internal DocConverter(int[] inputDocFileIndex) 
        {
            _docFieldsIndex = inputDocFileIndex;
        }

        // Specific documents is exported from 1C:DO or 1C:UPP spreadsheets
        internal void ConvertSpecificDocs(string[][] docsArr, string passedDocsReportPath)
        {
            int exceptCount = 0;

            for (int i = 0; i < docsArr.Length; i++)                                                        // Going through an array of documents
            {
                try
                {
                    _docObjects.Add((T)Activator
                               .CreateInstance(typeof(T), docsArr[i], _docFieldsIndex));   // Adding a document object to the list
                }
                catch
                {
                    exceptCount++;
                    if (exceptCount > this._maxPassedRows)
                        this._passedDocs.Add(docsArr[i]);                   // Adding document into error list 
                }
            }

            if (_passedDocs.Count > 0)
                CreateReportPassedDocs(passedDocsReportPath);
        }

                                                                            // Universal documents is exported from
                                                                            // this program spreadsheets
        public List<Document> ConvertUniversalDocs(string[][] ignoreArrDoDocuments)
        {
            List<Document> documents = new List<Document>(ignoreArrDoDocuments.Length);

            for (int i = 0; i < ignoreArrDoDocuments.Length; i++)
            {
                documents.Add(new Document(ignoreArrDoDocuments[i], this._docFieldsIndex));
            }
            return documents;
        }

        private void CreateReportPassedDocs(string exceptedUppPath)
        {
            this.PrintConsolePassedDocs(this._passedDocs);
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