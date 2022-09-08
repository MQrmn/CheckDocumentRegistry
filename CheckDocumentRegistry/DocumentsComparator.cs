using CheckDocumentRegistry.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDocumentRegistry
{
    public class DocumentsComparator
    {
        public string doSpreadSheet;
        public string uppSpreadSheet;
        public DocumentsComparator(ArgsParser args)
        {
            this.doSpreadSheet = args.doSpreadSheet;
            this.uppSpreadSheet = args.uppSpreadSheet;
        }


        public void Run()
        {
            SpreadSheetRepository spreadSheetRepository = new SpreadSheetRepository();

            //Console.WriteLine(this.doSpreadSheet + " " + this.uppSpreadSheet);


            DoDocumentConverter doDocumentConverter = new DoDocumentConverter();
            string[][] doDocumentsData = spreadSheetRepository.GetDocumentsFromTable(this.doSpreadSheet);
            List<DoDocument> doDocuments = doDocumentConverter.ConvertArrToDocuments(doDocumentsData);

            UppDocumentConverter uppDocumentConverter = new UppDocumentConverter();
            string[][] uppDocumentsData = spreadSheetRepository.GetDocumentsFromTable(this.uppSpreadSheet);
            List<UppDocument> uppDocuments = uppDocumentConverter.ConvertArrToDocuments(uppDocumentsData);

            Console.WriteLine(this.doSpreadSheet);

            foreach (var i in doDocuments)
                Console.WriteLine(i.docType + " - " + i.docDate + " - " + i.docSum + 
                                        " - " + i.docCounterparty + " - " + i.docNumber + " - " + i.docCompany);

            Console.WriteLine();

            Console.WriteLine(this.uppSpreadSheet);

            foreach (var i in uppDocuments)
                Console.WriteLine(i.docType + " - " + i.docDate + " - " + i.docSum + 
                                        " - " + i.docCounterparty + " - " + i.docNumber + " - " + i.docCompany);

            

        }
        
    }
}
