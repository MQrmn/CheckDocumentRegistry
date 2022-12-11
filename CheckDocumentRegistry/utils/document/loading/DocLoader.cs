
namespace RegComparator
{
    public class DocLoader
    {

        DocFieldsIndex docFieldsIndex = new();

        // Getting 1C:DO specific documents
        public List<Document> GetDocs1CDO(string spreadsheetPath, string passDocsPath)
        {
            string[][] docArrs1CDO = GetDocsFromFile(spreadsheetPath);

            DocConverter<Document1CDO> docsConverter = new(this.docFieldsIndex.DocFielsdIndex1CDO,
                                                           this.docFieldsIndex.CustomDocFieldsIndex1CDO,
                                                           this.docFieldsIndex.maxPassedRowForSwitch1CDO,
                                                           this.docFieldsIndex.rowLenght1CDO);
            List<Document> docObjs1CDO = docsConverter.ConvertSpecificDocs(docArrs1CDO, passDocsPath);
            return docObjs1CDO;
        }

        // Getting 1C:UPP specific documents
        public List<Document> GetDocs1CUPP(string spreadsheetPath, string exceptedDocsPath)
        {
            string[][] docArrs1CUPP = GetDocsFromFile(spreadsheetPath);

            DocConverter<Document1CUPP> docsConverter = new(this.docFieldsIndex.DocFieldsIndex1CUPP,
                                                            this.docFieldsIndex.CustomDocFieldsIndex1CUPP,
                                                            this.docFieldsIndex.maxPassedRowForSwitchUPP,
                                                            this.docFieldsIndex.rowLenght1CUPP);
            List<Document> docObjs1CUPP = docsConverter.ConvertSpecificDocs(docArrs1CUPP, exceptedDocsPath);

            return docObjs1CUPP;
        }

        // Get 1C:KA registry documents 
        public List<Document> GetDocs1CKA(string[] spreadsheetPathArr, string exceptedDocsPath)
        {
            
            List<Document> docObjs1CKA = new();
            foreach (var spreadsheetPath in spreadsheetPathArr)
            {
                string[][] docArrsTmp = GetDocsFromFile(spreadsheetPath);

                DocConverter<Document1CKA> docsConverter = new(this.docFieldsIndex.DocFieldsIndex1CKASf,
                                                               this.docFieldsIndex.CustomDocFieldsIndex1CKASf,
                                                               this.docFieldsIndex.maxPassedRowForSwitch1CKASf,
                                                               this.docFieldsIndex.rowLenght1CKASf);
                List<Document>  docObjs1CKATmp = docsConverter.ConvertSpecificDocs(docArrsTmp, exceptedDocsPath);
                docObjs1CKA = docObjs1CKA.Concat(docObjs1CKATmp).ToList();
            }
            

            return docObjs1CKA;
        }

        // Getting pass-through documents during comparison
        public List<Document> GetDocsPass(string spreadsheetPath)
        {
            List<Document> docObjs = new List<Document>();

            try
            {
                if (File.Exists(spreadsheetPath))
                {
                    string[][] ignore = GetDocsFromFile(spreadsheetPath);
                    DocConverter<Document> docsConverter = new(this.docFieldsIndex.docFieldsIndexUniversal);
                    docObjs = docsConverter.ConvertUniversalDocs(ignore);
                }
                else
                    Console.WriteLine($"Файл не найден: {spreadsheetPath}. Список игнорируемых документов не будет заполнен.");
            }
            catch
            {
                Console.WriteLine($"Не удалось прочитать файл: {spreadsheetPath}. Список игнорируемых документов не будет заполнен.");
            }
            return docObjs;
        }


        private string[][] GetDocsFromFile(string spreadsheetPath)
        {
            Console.WriteLine($"Чтение электронной таблицы: {spreadsheetPath}");
            SpreadSheetReaderXLSX spreadSheetReaderXLSX = new SpreadSheetReaderXLSX();
            return spreadSheetReaderXLSX.GetDocumentsFromTable(spreadsheetPath);
        }
    }
}
