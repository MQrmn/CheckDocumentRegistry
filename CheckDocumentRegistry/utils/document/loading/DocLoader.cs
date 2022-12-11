
namespace RegComparator
{
    public class DocLoader
    {

        DocFieldsIndex docFieldsIndex = new();

        // Getting 1C:DO specific documents
        internal protected List<Document> GetDocs1CDO(string spreadsheetPath, string passDocsPath)
        {
            string[][] docArrs1CDO = GetDocsFromWorker(spreadsheetPath);

            DocConverter<Document1CDO> docsConverter = new(this.docFieldsIndex.DocFielsdIndex1CDO,
                                                                           this.docFieldsIndex.CustomDocFieldsIndex1CDO,
                                                                     this.docFieldsIndex.maxPassedRowForSwitch1CDO,
                                                                                  this.docFieldsIndex.rowLenght1CDO);
            List<Document> docObjs1CDO = docsConverter.ConvertSpecificDocs(docArrs1CDO, passDocsPath);
            return docObjs1CDO;
        }

        // Getting 1C:UPP specific documents
        internal protected List<Document> GetDocs1CUPP(string spreadsheetPath, string exceptedDocsPath)
        {
            string[][] docArrs1CUPP = GetDocsFromWorker(spreadsheetPath);

            DocConverter<Document1CUPP> docsConverter = new(this.docFieldsIndex.DocFieldsIndex1CUPP,
                                                                            this.docFieldsIndex.CustomDocFieldsIndex1CUPP,
                                                                      this.docFieldsIndex.maxPassedRowForSwitchUPP,
                                                                                   this.docFieldsIndex.rowLenght1CUPP);
            List<Document> docObjs1CUPP = docsConverter.ConvertSpecificDocs(docArrs1CUPP, exceptedDocsPath);

            return docObjs1CUPP;
        }

        internal protected List<Document> GetDocs1CKASf(string spreadsheetPath, string exceptedDocsPath)
        {
            string[][] docArrs1KASf = GetDocsFromWorker(spreadsheetPath);

            DocConverter<Document1CKASf> docsConverter = new(this.docFieldsIndex.DocFieldsIndex1CKASf,
                                                                            this.docFieldsIndex.CustomDocFieldsIndex1CKASf,
                                                                      this.docFieldsIndex.maxPassedRowForSwitch1CKASf,
                                                                                   this.docFieldsIndex.rowLenght1CKASf);
            List<Document> docObjs1CKASf = docsConverter.ConvertSpecificDocs(docArrs1KASf, exceptedDocsPath);

            return docObjs1CKASf;
        }

        internal protected List<Document> GetDocs1CKATn(string spreadsheetPath, string exceptedDocsPath)
        {
            string[][] docArrs1KASf = GetDocsFromWorker(spreadsheetPath);

            DocConverter<Document1CKATn> docsConverter = new(this.docFieldsIndex.DocFieldsIndex1CKATn,
                                                                            this.docFieldsIndex.CustomDocFieldsIndex1CKATn,
                                                                      this.docFieldsIndex.maxPassedRowForSwitch1CKATn,
                                                                                   this.docFieldsIndex.rowLenght1CKATn);
            List<Document> docObjs1CKATn = docsConverter.ConvertSpecificDocs(docArrs1KASf, exceptedDocsPath);

            return docObjs1CKATn;
        }

        // Getting pass-through documents during comparison
        internal protected List<Document> GetDocsPass(string spreadsheetPath)
        {
            List<Document> docObjs = new List<Document>();

            try
            {
                if (File.Exists(spreadsheetPath))
                {
                    string[][] ignore = GetDocsFromWorker(spreadsheetPath);
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


        internal protected string[][] GetDocsFromWorker(string spreadsheetPath)
        {
            Console.WriteLine($"Чтение электронной таблицы: {spreadsheetPath}");
            SpreadSheetReaderXLSX spreadSheetReaderXLSX = new SpreadSheetReaderXLSX();
            return spreadSheetReaderXLSX.GetDocumentsFromTable(spreadsheetPath);
        }
    }
}
