
namespace CheckDocumentRegistry
{
    internal class Program
    {
        static void Main()
        {
            string folder = "All";
            string[] args = new string[6];
            args[0] = "-do";
            args[1] = $"C:\\1C\\{folder}\\DocumentReportDO.xlsx";
            args[2] = "-upp";
            args[3] = $"C:\\1C\\{folder}\\DocumentReportUPP.xlsx";
            args[4] = "-o";
            args[5] = $"C:\\1C\\output.xlsx";

            ArgsParser parsedArgs = new ArgsParser(args);

            if (parsedArgs.doSpreadSheet != null && parsedArgs.uppSpreadSheet != null)
            {
                DocumentsComparator documentsComparator = new DocumentsComparator(parsedArgs);
                documentsComparator.Run();
            }
            
        }
    }
}