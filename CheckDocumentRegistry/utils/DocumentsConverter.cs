
using System.Runtime.CompilerServices;

namespace CheckDocumentRegistry
{
    internal class DocumentsConverter
    {
        private int[] docFieldIndex1CDo;            // Standard 1C:DO document format
        private int[] customDocFieldIndex1CDo;      // Simplified 1C:DO document format
        private int[] docFieldIndex1CUpp;           // Standard 1C:UPP document format
        private int[] customDocFieldIndex1CUpp;     // Simplified 1C:UPP document format
        List<string[]> exceptedDocuments;

        internal DocumentsConverter()
        {
            this.docFieldIndex1CDo = new int[] { 0, 3, 6, 7, 8, 9, 10, 11 };
            this.customDocFieldIndex1CDo = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            this.docFieldIndex1CUpp = new int[] { 0, 1, 3, 4, 5, 6, 7 };
            this.customDocFieldIndex1CUpp = new int[] { 0, 1, 2, 3, 4, 5, 6 };
            this.exceptedDocuments = new List<string[]>();
        }


        public List<Document> Convert1CDoDocuments(string[][] input, string exceptedDoPath)
        {
            int[] fieldIndexes = docFieldIndex1CDo;
            List<Document1CDo> doDocuments = new List<Document1CDo>(input.Length);

            int numberOfException = new();
            bool isSwithedByException = false; 
            for (int i = 0; i < input.Length; i++)
            {
                try
                {
                    doDocuments.Add(new Document1CDo(input[i], fieldIndexes));
                    numberOfException = 0;
                }
                catch 
                {
                    numberOfException++;

                    if ((numberOfException > 7 && isSwithedByException == false) || isSwithedByException == true)
                    {
                        this.exceptedDocuments.Add(input[i]);
                    }

                    if (numberOfException >= 7 && isSwithedByException == false && input[i].Length < 12)
                    {
                        isSwithedByException = true;
                        numberOfException = 0;
                        i = -1;
                        fieldIndexes = customDocFieldIndex1CDo;
                    }
                }
            }

            if (exceptedDocuments.Count > 0)
            {
                this.PrintExceptedDocuments(this.exceptedDocuments);
                SpreadSheetWriterXLSX spreadSheetWriterXLSX = new SpreadSheetWriterXLSX(exceptedDoPath);
                spreadSheetWriterXLSX.CreateSpreadsheet(exceptedDocuments);
            }
            

            List<Document> documents = doDocuments
                .ConvertAll(new Converter<Document1CDo, Document>(delegate (Document1CDo document) {
                    return (Document)document;
                }));

            return documents;
        }

        public List<Document> Convert1CUppDocuments(string[][] input, string exceptedUppPath)
        {
            int[] fieldIndex = docFieldIndex1CUpp;
            List<Document1CUpp> doDocuments = new List<Document1CUpp>(input.Length);

            int numberOfException = new();
            bool isSwithedByException = false;
            for (int i = 0; i < input.Length; i++)
            {
                try
                {
                    doDocuments.Add(new Document1CUpp(input[i], fieldIndex));
                }
                catch
                {
                    numberOfException++;

                    if ((numberOfException > 1 && isSwithedByException == false) || isSwithedByException == true)
                    {
                        this.exceptedDocuments.Add(input[i]);
                    }

                    if (numberOfException >= 1 && isSwithedByException == false && input[i].Length < 8)
                    {
                        isSwithedByException = true;
                        numberOfException = 0;
                        i = -1;
                        fieldIndex = customDocFieldIndex1CUpp;
                    }
                }
            }

            if(exceptedDocuments.Count > 0)
            {
                this.PrintExceptedDocuments(this.exceptedDocuments);
                SpreadSheetWriterXLSX spreadSheetWriterXLSX = new SpreadSheetWriterXLSX(exceptedUppPath);
                spreadSheetWriterXLSX.CreateSpreadsheet(exceptedDocuments);
            }

            List<Document> documents = doDocuments
                .ConvertAll(new Converter<Document1CUpp, Document>(delegate (Document1CUpp document) {
                    return (Document)document;
                }));

            return documents;
        }


        public List<Document> ConvertIgnoreDoc(string[][] input)
        {
            List<Document> documents = new List<Document>(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                try
                {
                    documents.Add(new Document(input[i]));
                }
                catch { }
            }
            return documents;
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