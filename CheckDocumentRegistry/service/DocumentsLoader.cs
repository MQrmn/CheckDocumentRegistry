using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDocumentRegistry
{
    internal class DocumentsLoader
    {

        List<DoDocument> doDocuments = new List<DoDocument>();
        List<UppDocument> uppDocuments = new List<UppDocument>();
        SpreadSheetRepository spreadSheetRepository = new SpreadSheetRepository();
        DoDocumentConverter doDocumentConverter = new DoDocumentConverter();
        UppDocumentConverter uppDocumentConverter = new UppDocumentConverter();



        public List<DoDocument> GetDoDocuments(string filePath)
        {
            Console.WriteLine($"Reading Table {filePath} ");
            string[][] doDocumentsData = this.spreadSheetRepository.GetDocumentsFromTable(filePath);

            Console.WriteLine("Converting Do data");
            this.doDocuments = this.doDocumentConverter.ConvertArrToDocuments(doDocumentsData);

            return this.doDocuments;
        }

        public List<UppDocument> GetUppDocuments(string filePath)
        {
            Console.WriteLine($"Reading Table {filePath} ");
            string[][] uppDocumentsData = this.spreadSheetRepository.GetDocumentsFromTable(filePath);

            Console.WriteLine("Converting Upp data");
            this.uppDocuments = this.uppDocumentConverter.ConvertArrToDocuments(uppDocumentsData);

            return this.uppDocuments;
        }

    }
}
