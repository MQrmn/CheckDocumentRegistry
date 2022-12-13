namespace RegComparator
{
    public class DocLoader
    {
        private DocFieldsBase _docFieldsSettings;
        private List<Document> _documents;

        public DocLoader() { }
        public DocLoader(DocFieldsBase docFields, List<Document> documents)
        {
            _docFieldsSettings = docFields;
            _documents = documents;
        }

        DocFieldsSettings docFieldsSettings = new();


        public void GetDocObjectList<T>(string spreadsheetPath, string passDocsPath) where T: Document
        {
            string[][] docArrs = GetDocsFromFile(spreadsheetPath);

            DocConverter<T> docsConverter = new(_docFieldsSettings.DocFielsdIndex,
                                                _docFieldsSettings.MaxPassedRows,
                                                _docFieldsSettings.RowLenght);

            _documents = docsConverter.ConvertSpecificDocs(docArrs, passDocsPath);
        }


        // Getting 1C:DO specific documents
        public List<Document> GetDocs1CDO(string spreadsheetPath, string passDocsPath)
        {
            string[][] docArrs1CDO = GetDocsFromFile(spreadsheetPath);

            DocConverter<Document1CDO> docsConverter = new(docFieldsSettings.DocFielsdIndex1CDO,
                                                           docFieldsSettings.MaxPassedRowForSwitch1CDO,
                                                           docFieldsSettings.RowLenght1CDO);
            List<Document> docObjs1CDO = docsConverter.ConvertSpecificDocs(docArrs1CDO, passDocsPath);
            return docObjs1CDO;
        }

        // Getting 1C:UPP specific documents
        public List<Document> GetDocs1CUPP(string spreadsheetPath, string exceptedDocsPath)
        {
            string[][] docArrs1CUPP = GetDocsFromFile(spreadsheetPath);

            DocConverter<Document1CUPP> docsConverter = new(docFieldsSettings.DocFieldsIndex1CUPP,
                                                            docFieldsSettings.MaxPassedRowForSwitchUPP,
                                                            docFieldsSettings.RowLenght1CUPP);
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

                DocConverter<Document1CKA> docsConverter = new(docFieldsSettings.DocFieldsIndex1CKA,
                                                               docFieldsSettings.MaxPassedRowForSwitch1CKA,
                                                               docFieldsSettings.RowLenght1CKA);
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
                    DocConverter<Document> docsConverter = new(this.docFieldsSettings.docFieldsIndexCommon);
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
