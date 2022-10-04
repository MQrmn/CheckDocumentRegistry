
using System.Runtime.CompilerServices;

namespace CheckDocumentRegistry
{
    internal class DocumentsConverter
    {
        private int[] docFieldIndex1CDo;            // Standard 1C:DO document format
        private int[] customDocFieldIndex1CDo;      // Simplified 1C:DO document format
        private int[] docFieldIndex1CUpp;           // Standard 1C:UPP document format
        private int[] customDocFieldIndex1CUpp;     // Simplified 1C:UPP document format
        internal List<string[]> ExceptedDocuments;

        internal DocumentsConverter()
        {
            this.docFieldIndex1CDo = new int[] { 0, 3, 6, 7, 8, 9, 10, 11 };
            this.customDocFieldIndex1CDo = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };

            this.docFieldIndex1CUpp = new int[] { 0, 1, 3, 4, 5, 6, 7 };
            this.customDocFieldIndex1CUpp = new int[] { 0, 1, 2, 3, 4, 5, 6 };

            this.ExceptedDocuments = new List<string[]>();
        }


        public List<Document> Convert1CDoDocuments(string[][] documentsArrDo, string exceptedDoPath)
        {
            
            List<Document1CDo> doDocuments = new List<Document1CDo>(documentsArrDo.Length);

            int numberOfExceptions = 0;
            bool isSwithedByException = false;
            int[] fieldIndexes = docFieldIndex1CDo;

            for (int i = 0; i < documentsArrDo.Length; i++)
            {
                try
                {
                    doDocuments.Add(new Document1CDo(documentsArrDo[i], fieldIndexes));
                    numberOfExceptions = 0;
                }
                catch 
                {
                    numberOfExceptions++;

                    if ((numberOfExceptions > 7 && isSwithedByException == false) || isSwithedByException == true)
                    {
                        this.ExceptedDocuments.Add(documentsArrDo[i]);
                    }

                    if (numberOfExceptions >= 7 && isSwithedByException == false && documentsArrDo[i].Length < 12)
                    {
                        isSwithedByException = true;
                        numberOfExceptions = 0;
                        i = -1;
                        fieldIndexes = customDocFieldIndex1CDo;
                    }
                }
            }

            if (ExceptedDocuments.Count > 0)
                CreateExceptedDocumentReport(exceptedDoPath);

            List<Document> documents = doDocuments
                .ConvertAll(new Converter<Document1CDo, Document>(delegate (Document1CDo document) {
                    return (Document)document;
                }));

            return documents;
        }

        public List<Document> Convert1CUppDocuments(string[][] documentsArrUpp, string exceptedUppPath)
        {
            List<Document1CUpp> uppDocuments = new List<Document1CUpp>(documentsArrUpp.Length);

            int numberOfExceptions = 0;
            bool isSwithedByException = false;
            int[] fieldIndex = docFieldIndex1CUpp;

            for (int i = 0; i < documentsArrUpp.Length; i++)
            {
                try
                {
                    uppDocuments.Add(new Document1CUpp(documentsArrUpp[i], fieldIndex));
                }
                catch
                {
                    numberOfExceptions++;

                    if ((numberOfExceptions > 1 && isSwithedByException == false) || isSwithedByException == true)
                    {
                        this.ExceptedDocuments.Add(documentsArrUpp[i]);
                    }

                    if (numberOfExceptions >= 1 && isSwithedByException == false && documentsArrUpp[i].Length < 8)
                    {
                        isSwithedByException = true;
                        numberOfExceptions = 0;
                        i = -1;
                        fieldIndex = customDocFieldIndex1CUpp;
                    }
                }
            }

            if(ExceptedDocuments.Count > 0)
                CreateExceptedDocumentReport(exceptedUppPath);

            List<Document> documents = uppDocuments
                .ConvertAll(new Converter<Document1CUpp, Document>(delegate (Document1CUpp document) {
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