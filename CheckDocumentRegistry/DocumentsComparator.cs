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

        List<DoDocument> doDocuments = new List<DoDocument>();
        List<UppDocument> uppDocuments = new List<UppDocument>();

        List<DoDocument> catchedDoDocuments = new List<DoDocument>();
        List<UppDocument> catchedUppDocuments = new List<UppDocument>();

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
            UppDocumentConverter uppDocumentConverter = new UppDocumentConverter();

            Console.WriteLine($"Reading Table {this.doSpreadSheet} ");
            string[][] doDocumentsData = spreadSheetRepository.GetDocumentsFromTable(this.doSpreadSheet);
            Console.WriteLine($"Reading table {this.uppSpreadSheet}");
            string[][] uppDocumentsData = spreadSheetRepository.GetDocumentsFromTable(this.uppSpreadSheet);
            Console.WriteLine("Converting Do data");
            this.doDocuments = doDocumentConverter.ConvertArrToDocuments(doDocumentsData);
            Console.WriteLine("Converting Upp data");
            this.uppDocuments = uppDocumentConverter.ConvertArrToDocuments(uppDocumentsData);
            
            Console.WriteLine("Comparing documents");
            this.doDocuments.ForEach(this.CompareDocuments);

            Console.WriteLine("Preparing list of uncatched documents in Do");
            this.ClearSourseDoList(this.doDocuments, this.catchedDoDocuments);
            Console.WriteLine("Preparing list of uncatched documents in Upp");
            this.ClearSourseUppList(this.uppDocuments, this.catchedUppDocuments);

            int j = 0;

            Console.WriteLine();

            foreach (var i in this.catchedDoDocuments)
            {
                j++;
            }
            Console.WriteLine($"catch. inDoo {j}");
            j = 0;

            Console.WriteLine();
      

            foreach (var i in this.catchedUppDocuments)
            {
                j++;
            }
            Console.WriteLine($"catch. inUpp {j}");
            j = 0;
            
            Console.WriteLine();
            //Console.WriteLine("uncatch. inDo ");
            foreach (var i in doDocuments)
            {
                j++;
            }
            Console.WriteLine($"uncatch. inDo {j}");

            j = 0;

            foreach (var i in uppDocuments)
            {
                j++;

            }


            List<Document> convertedCatchedUppDocuments = this.catchedUppDocuments
                .ConvertAll(new Converter<UppDocument, Document>(delegate (UppDocument document) {
                    return (Document)document;
                }));

            List<Document> convertedCatchedDoDocuments = this.catchedDoDocuments
                .ConvertAll(new Converter<DoDocument, Document>(delegate (DoDocument document) {
                    return (Document)document;
                }));

            List<Document> convertedUnCatchedUppDocuments = this.uppDocuments
                .ConvertAll(new Converter<UppDocument, Document>(delegate (UppDocument document) {
                    return (Document)document;
                }));

            List<Document> convertedUnCatchedDoDocuments = this.doDocuments
                .ConvertAll(new Converter<DoDocument, Document>(delegate (DoDocument document) {
                    return (Document)document;
                }));


            Console.WriteLine($"uncatch. inUpp {j}");

            ReportCreator reportCreator = new ReportCreator();
            reportCreator.MakeReport("C:\\1C\\МatchedUppDocuments.xlsx", convertedCatchedUppDocuments);
            reportCreator.MakeReport("C:\\1C\\МatchedDoDocuments.xlsx", convertedCatchedDoDocuments);
            reportCreator.MakeReport("C:\\1C\\UnМatchedUppDocuments.xlsx", convertedUnCatchedUppDocuments);
            reportCreator.MakeReport("C:\\1C\\UnМatchedDoDocuments.xlsx", convertedUnCatchedDoDocuments);

        }

        private void CompareDocuments(DoDocument doDocument)
        {
            List<UppDocument> firstCatchedUpp = this.uppDocuments.FindAll(
             delegate(UppDocument document)
             {
                 return CatchDoDocumentInUpp(document, doDocument);
             });

            firstCatchedUpp.ForEach(delegate (UppDocument document)
            {
                this.CatchRalatedUppDocument(document);
            });
        }
        
        private bool CatchDoDocumentInUpp(UppDocument uppDocument, DoDocument doDocument)
        {
            bool result = uppDocument.docType == doDocument.docType
                && uppDocument.docNumber == doDocument.docNumber
                && uppDocument.docSum == doDocument.docSum;

            if (result)
            {
                if (doDocument.isUpd) uppDocument.isUpd = doDocument.isUpd;

                this.catchedDoDocuments.Add(doDocument);
                this.catchedUppDocuments.Add(uppDocument);
            }
            
            return result;
        }

        private void CatchRalatedUppDocument(UppDocument catchedUppDocument)
        {
            List<UppDocument> result = this.uppDocuments.FindAll(
                delegate(UppDocument document)
                {
                    bool result = document.docType != catchedUppDocument.docType
                            && document.docNumber == catchedUppDocument.docNumber
                            && document.docSum == catchedUppDocument.docSum
                            && catchedUppDocument.isUpd == true;

                    if (result) document.isUpd = true;

                    return result;
                });

            result.ForEach(delegate (UppDocument document)
            {
                this.catchedUppDocuments.Add(document);
            });
        }

        private void ClearSourseDoList(List<DoDocument> sourceDocumentList, List<DoDocument> catchedDocumentList)
        {
            catchedDocumentList.ForEach(delegate(DoDocument document)
            {
                sourceDocumentList.Remove(document);
            });
        }

        private void ClearSourseUppList(List<UppDocument> sourceDocumentList, List<UppDocument> catchedDocumentList)
        {
            catchedDocumentList.ForEach(delegate (UppDocument document)
            {
                sourceDocumentList.Remove(document);
            });
        }
    }
}
