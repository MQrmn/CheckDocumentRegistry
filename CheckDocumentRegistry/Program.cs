
namespace CheckDocumentRegistry
{
    internal class Program
    {
        static void Main()
        {
            string[] args = new string[6];
            args[0] = "-do";
            args[1] = @"C:\1C\DocumentReportDO.xlsx";
            args[2] = "-upp";
            args[3] = @"C:\1C\DocumentReportUPP.xlsx";
            args[4] = "-o";
            args[5] = @"C:\1C\Output.xlsx";

            ArgsParser parsedArgs = new ArgsParser(args);

            if (parsedArgs.doSpreadSheet != null && parsedArgs.uppSpreadSheet != null)
            {
                DocumentsComparator documentsComparator = new DocumentsComparator(parsedArgs);
                documentsComparator.Run();
            }
                

            

            
        }
    }
}