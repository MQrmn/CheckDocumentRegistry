namespace RegComparator
{
    public class ConsoleWriter : IUserReporter
    {
        public void ReportInfo(string message) => Console.WriteLine(message);
        public void ReportSpecial(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public void ReportError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
            
            
    }
}
