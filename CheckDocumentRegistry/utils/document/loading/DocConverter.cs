namespace RegComparator
{

    internal class DocConverter<T> where T : Document
    {
        internal List<string[]>? _passedDocs;
        private int[] _docFieldsIndexStandard;
        //private int[] docFieldsIndexCustom;
        private int _maxPassedRowForSwitchIndex;
        private int _rowLenght;

        private List<Document> _universalDocs;

        internal DocConverter(int[] inputDocFileIndexStandard,
                                 int inputMaxPassedRowForSwitchIndex,
                                 int inputRowLength,
                                 List<Document> universalDocs)
        {
            _docFieldsIndexStandard = inputDocFileIndexStandard;
            _passedDocs = new List<string[]>();
            _maxPassedRowForSwitchIndex = inputMaxPassedRowForSwitchIndex;
            _rowLenght = inputRowLength;
            _universalDocs = universalDocs;
        }

        internal DocConverter(int[] inputDocFileIndex) 
        {
            this._docFieldsIndexStandard = inputDocFileIndex;
        }

        // Specific documents is exported from 1C:DO or 1C:UPP spreadsheets
        internal void ConvertSpecificDocs(string[][] docsArr, string passedDocsReportPath)
        {
            List<T> specificDocObjList = new List<T>(docsArr.Length);

            int exceptCount = 0;
                                                                            //bool isSwithedIndex = false;
            int[] fieldIndexes = this._docFieldsIndexStandard;

            for (int i = 0; i < docsArr.Length; i++)                        // Going through an array of documents
            {
                try
                {
                                                                            // Adding a document object to the list
                    _universalDocs.Add((T)Activator.CreateInstance(typeof(T), docsArr[i], fieldIndexes));   
                }
                catch
                {
                    exceptCount++;
                                                                            // Adding document into error list
                    if (exceptCount > this._maxPassedRowForSwitchIndex)
                        this._passedDocs.Add(docsArr[i]);                    
                }
            }

            if (_passedDocs.Count > 0)
                CreateReportPassedDocs(passedDocsReportPath);

            Console.WriteLine("DocConverter" + _universalDocs.GetHashCode());
        }

                                                                            // Universal documents is exported from
                                                                            // this program spreadsheets
        public List<Document> ConvertUniversalDocs(string[][] ignoreArrDoDocuments)
        {
            List<Document> documents = new List<Document>(ignoreArrDoDocuments.Length);

            for (int i = 0; i < ignoreArrDoDocuments.Length; i++)
            {
                documents.Add(new Document(ignoreArrDoDocuments[i], this._docFieldsIndexStandard));
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