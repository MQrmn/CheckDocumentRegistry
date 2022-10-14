namespace RegComparator
{

    internal class DocConverter<T> where T : Document
    {
        internal List<string[]>? PassedDocs;
        private int[] docFieldsIndexStandard;
        private int[] docFieldsIndexCustom;
        private int maxPassedRowForSwitchIndex;
        private int rowLenght;


        internal DocConverter(    int[] inputDocFileIndexStandard, 
                                        int[] inputDocFileIndexCustom,
                                        int inputMaxPassedRowForSwitchIndex,
                                        int inputRowLength )
        {
            this.docFieldsIndexStandard = inputDocFileIndexStandard;
            this.docFieldsIndexCustom = inputDocFileIndexCustom;
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
            bool isSwithedIndex = false;
            int[] fieldIndexes = this.docFieldsIndexStandard;

            for (int i = 0; i < docsArr.Length; i++)
            {
                try
                {
                    specificDocObjList.Add((T)Activator.CreateInstance(typeof(T), docsArr[i], fieldIndexes));

                    exceptCount = 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    exceptCount++;

                    if ((exceptCount > this.maxPassedRowForSwitchIndex && isSwithedIndex == false) || isSwithedIndex == true)
                    {
                        this.PassedDocs.Add(docsArr[i]);
                    }

                    if (exceptCount >= this.maxPassedRowForSwitchIndex && isSwithedIndex == false && docsArr[i].Length < this.rowLenght)
                    {
                        isSwithedIndex = true;
                        exceptCount = 0;
                        i = -1;
                        fieldIndexes = this.docFieldsIndexCustom; // Switch fields index set
                    }
                }
            }

            if (PassedDocs.Count > 0)
                CreateReportPassedDocs(passedDocsReportPath);

            List<Document> universalDocs = specificDocObjList
                .ConvertAll(new Converter<T, Document>(
                (T document) => (Document)document
                ));

            return universalDocs;
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