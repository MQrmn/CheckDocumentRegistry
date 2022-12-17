namespace RegComparator
{
    public class ConsoleWriter : IUserReporter
    {
        public void ReportInfo(object sender, string message) => Console.WriteLine(message);
        public void ReportSpecial(object sender, string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public void ReportError(object sender, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
            
            
    }
}
