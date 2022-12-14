namespace RegComparator
{

    internal class DocConverter<T> where T : Document
    {
        internal List<string[]>? PassedDocs;
        private int[] docFieldsIndexStandard;
        //private int[] docFieldsIndexCustom;
        private int maxPassedRowForSwitchIndex;
        private int rowLenght;

        private List<Document> _universalDocs;

        internal DocConverter(int[] inputDocFileIndexStandard,
                                 int inputMaxPassedRowForSwitchIndex,
                                 int inputRowLength,
                                 List<Document> universalDocs)
        {
            this.docFieldsIndexStandard = inputDocFileIndexStandard;
            this.PassedDocs = new List<string[]>();
            this.maxPassedRowForSwitchIndex = inputMaxPassedRowForSwitchIndex;
            this.rowLenght = inputRowLength;
            _universalDocs = universalDocs;
        }

        internal DocConverter(    int[] inputDocFileIndexStandard, 
                                        int inputMaxPassedRowForSwitchIndex,
                                        int inputRowLength )
        {
            this.docFieldsIndexStandard = inputDocFileIndexStandard;
            this.PassedDocs = new List<string[]>();
            this.maxPassedRowForSwitchIndex = inputMaxPassedRowForSwitchIndex;
            this.rowLenght = inputRowLength;
        }


        internal DocConverter(int[] inputDocFileIndex) 
        {
            this.docFieldsIndexStandard = inputDocFileIndex;
        }


        // Specific documents is exported from 1C:DO or 1C:UPP spreadsheets
        internal List<Document> ConvertSpecificDocs(string[][] docsArr, string passedDocsReportPath)
        {
            List<T> specificDocObjList = new List<T>(docsArr.Length);

            int exceptCount = 0;
            //bool isSwithedIndex = false;
            int[] fieldIndexes = this.docFieldsIndexStandard;

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

                    if (exceptCount > this.maxPassedRowForSwitchIndex)
                        this.PassedDocs.Add(docsArr[i]);                    // Adding document into error list
                    

                }
            }

            if (PassedDocs.Count > 0)
                CreateReportPassedDocs(passedDocsReportPath);

            Console.WriteLine("DocConverter 1 " + _universalDocs.GetHashCode());
            //_universalDocs = specificDocObjList.ConvertAll(new Converter<T, Document>((T document) => (Document)document));
            Console.WriteLine("DocConverter 2 " + _universalDocs.GetHashCode());
            return _universalDocs;
        }


        // Universal documents is exported from this program spreadsheets
        public List<Document> ConvertUniversalDocs(string[][] ignoreArrDoDocuments)
        {
            List<Document> documents = new List<Document>(ignoreArrDoDocuments.Length);

            for (int i = 0; i < ignoreArrDoDocuments.Length; i++)
            {
                documents.Add(new Document(ignoreArrDoDocuments[i], this.docFieldsIndexStandard));
            }
            return documents;
        }


        private void CreateReportPassedDocs(string exceptedUppPath)
        {
            this.PrintConsolePassedDocs(this.PassedDocs);
            SpreadSheetWriterXLSX spreadSheetWriterXLSX = new SpreadSheetWriterXLSX(exceptedUppPath);
            spreadSheetWriterXLSX.CreateSpreadsheet(PassedDocs);
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