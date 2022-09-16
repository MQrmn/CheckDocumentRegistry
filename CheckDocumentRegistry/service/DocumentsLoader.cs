
namespace CheckDocumentRegistry
{
    public class DocumentsLoader
    {
        SpreadSheetRepository spreadSheetRepository = new SpreadSheetRepository();
        public List<Document>? doDocuments { get; }
        public List<Document>? uppDocuments { get; }

        public DocumentsLoader(string doPath, string uppPath)
        {
            Abort(doPath);
            Abort(uppPath);

            this.doDocuments = this.GetDoDocuments(doPath);
            this.uppDocuments = this.GetUppDocuments(uppPath);
        }

        void Abort(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Abort: File \"{path}\" not found.");
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
    }
}
