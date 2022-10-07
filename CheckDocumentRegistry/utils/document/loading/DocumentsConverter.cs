
using System.Runtime.CompilerServices;

namespace CheckDocumentRegistry
{
    internal class DocumentsConverter<T> where T : Document
    {
        internal List<string[]>? ExceptedDocuments;

        private int[] docFileIndexStandard;
        private int[] docFileIndexCustom;
        private int maxPassedRowForSwitchIndex;
        private int rowLength;


        internal DocumentsConverter( int[] inputDocFileIndexStandard, 
                                        int[] inputDocFileIndexCustom,
                                        int inputMaxPassedRowForSwitchIndex,
                                        int inputRowLength
                                        )
        {
            this.docFileIndexStandard = inputDocFileIndexStandard;
            this.docFileIndexCustom = inputDocFileIndexCustom;
            this.ExceptedDocuments = new List<string[]>();
            this.maxPassedRowForSwitchIndex = inputMaxPassedRowForSwitchIndex;
            this.rowLength = inputRowLength;
        }

        internal DocumentsConverter() { }

        internal List<Document> ConvertDocuments(string[][] documentsArr, string exceptedDoPath)
        {
            List<T> preDocuments = new List<T>(documentsArr.Length);

            int numberOfExceptions = 0;
            bool isSwithedByException = false;
            int[] fieldIndexes = this.docFileIndexStandard;

            for (int i = 0; i < documentsArr.Length; i++)
            {
                try
                {
                    preDocuments.Add((T)Activator.CreateInstance(typeof(T), documentsArr[i], fieldIndexes));
                    numberOfExceptions = 0;
                }
                catch
                {
                    numberOfExceptions++;

                    if ((numberOfExceptions > this.maxPassedRowForSwitchIndex && isSwithedByException == false) || isSwithedByException == true)
                    {
                        this.ExceptedDocuments.Add(documentsArr[i]);
                    }

                    if (numberOfExceptions >= this.maxPassedRowForSwitchIndex && isSwithedByException == false && documentsArr[i].Length < this.rowLength)
                    {
                        isSwithedByException = true;
                        numberOfExceptions = 0;
                        i = -1;
                        fieldIndexes = this.docFileIndexCustom;
                    }
                }
            }

            if (ExceptedDocuments.Count > 0)
                CreateExceptedDocumentReport(exceptedDoPath);

            List<Document> documents = preDocuments
                .ConvertAll(new Converter<T, Document>(delegate (T document) {
                    return (Document)document;
                }));

            return documents;


        }


        public List<Document> ConvertIgnoreDoc(string[][] ignoreArrDoDocuments)
        {
            List<Document> documents = new List<Document>(ignoreArrDoDocuments.Length);

            for (int i = 0; i < ignoreArrDoDocuments.Length; i++)
            {
                documents.Add(new Document(ignoreArrDoDocuments[i]));
                
            }
            return documents;
        }


        private void CreateExceptedDocumentReport(string exceptedUppPath)
        {
            this.PrintExceptedDocuments(this.ExceptedDocuments);
            SpreadSheetWriterXLSX spreadSheetWriterXLSX = new SpreadSheetWriterXLSX(exceptedUppPath);
            spreadSheetWriterXLSX.CreateSpreadsheet(ExceptedDocuments);
        }


        private void PrintExceptedDocuments(List<string[]> exceptedDocuments)
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