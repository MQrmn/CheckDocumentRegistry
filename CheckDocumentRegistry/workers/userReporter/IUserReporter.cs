namespace RegComparator
{
    internal interface IUserReporter
    {
        public void ReportInfo(string message);
        public void ReportSpecial(string message);
        public void ReportError(string message);
    }
}
