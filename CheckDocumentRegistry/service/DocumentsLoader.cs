
namespace CheckDocumentRegistry
{
    public class DocumentsLoader
    {
        SpreadSheetRepository spreadSheetRepository = new SpreadSheetRepository();
        public List<Document>? doDocuments { get; }
        public List<Document>? uppDocuments { get; }
        public List<Document>? ignoreDoDocuments { get; }

        public DocumentsLoader(Arguments args)
        {
            TryAbort(args);

            this.doDocuments = this.GetDoDocuments(args.doSpreadSheetPath);
            this.uppDocuments = this.GetUppDocuments(args.uppSpreadSheetPath);
            this.ignoreDoDocuments = this.GetIgnoreDo(args.doIgnoreSpreadSheetPath);

        }

        void TryAbort(Arguments args)
        {
            bool doExist = File.Exists(args.doSpreadSheetPath);
            bool uppExist = File.Exists(args.uppSpreadSheetPath);

            if (doExist && uppExist)
            {
                return;
            }
            else
            {
                if (!doExist)
                {
                    Console.WriteLine($"Abort: File \"{args.doSpreadSheetPath}\" not found.");
                }
                else if (!uppExist)
                {
                    Console.WriteLine($"Abort: File \"{args.uppSpreadSheetPath}\" not found.");
                }
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public List<Document> GetDoDocuments(string filePath)
        {
            Console.WriteLine($"Reading Table {filePath}");
            string[][] doDocumentsData = this.spreadSheetRepository.GetDocumentsFromTable(filePath);

            Console.WriteLine("Converting Do data");
            List<Document> documents = Converter.ConvertDoDocuments(doDocumentsData);

            return documents;
        }

        public List<Document> GetUppDocuments(string filePath)
        {
            Console.WriteLine($"Reading Table {filePath}");
            string[][] uppDocumentsData = this.spreadSheetRepository.GetDocumentsFromTable(filePath);

            Console.WriteLine("Converting Upp data");
            List<Document> documents = Converter.ConvertUppDocuments(uppDocumentsData);

            return documents;
        }

        List<Document> GetIgnoreDo(string filePath)
        {
            Console.WriteLine($"Reading Table {filePath}");
            string[][] ignore = this.spreadSheetRepository.GetDocumentsFromTable(filePath);
            List<Document> documents = Converter.ConvertIgnoreDoc(ignore);
            return documents;
        }
    }
}
