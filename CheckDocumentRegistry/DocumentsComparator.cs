using CheckDocumentRegistry.service;

namespace CheckDocumentRegistry
{
    public class DocumentsComparator
    {
        string doSpreadSheet;
        string uppSpreadSheet;

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

            DocumentsLoader loader = new DocumentsLoader();
            SpreadSheetRepository spreadSheetRepository = new SpreadSheetRepository();

            this.doDocuments = loader.GetDoDocuments(this.doSpreadSheet);
            this.uppDocuments = loader.GetUppDocuments(this.uppSpreadSheet);



            Console.WriteLine("Comparing documents");
            this.doDocuments.ForEach(this.CompareDocuments);

            Console.WriteLine("Preparing list of uncatched documents in Do");
            this.ClearSourseDoList(this.doDocuments, this.catchedDoDocuments);

            Console.WriteLine("Preparing list of uncatched documents in Upp");
            this.ClearSourseUppList(this.uppDocuments, this.catchedUppDocuments);


            List<Document> convMatchedUppDocuments = this.catchedUppDocuments
                .ConvertAll(new Converter<UppDocument, Document>(delegate (UppDocument document) {
                    return (Document)document;}));

            List<Document> convtMatchedDoDocuments = this.catchedDoDocuments
                .ConvertAll(new Converter<DoDocument, Document>(delegate (DoDocument document) {
                    return (Document)document;}));

            List<Document> convUnMatchedUppDocuments = this.uppDocuments
                .ConvertAll(new Converter<UppDocument, Document>(delegate (UppDocument document) {
                    return (Document)document;}));

            List<Document> convUnCatchedDoDocuments = this.doDocuments
                .ConvertAll(new Converter<DoDocument, Document>(delegate (DoDocument document) {
                    return (Document)document;}));


            ReportCreator reportCreator = new ReportCreator();

            reportCreator.MakeReport("C:\\1C\\МatchedUppDocuments.xlsx", convMatchedUppDocuments);
            reportCreator.MakeReport("C:\\1C\\МatchedDoDocuments.xlsx", convtMatchedDoDocuments);
            reportCreator.MakeReport("C:\\1C\\UnМatchedUppDocuments.xlsx", convUnMatchedUppDocuments);
            reportCreator.MakeReport("C:\\1C\\UnМatchedDoDocuments.xlsx", convUnCatchedDoDocuments);
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
