namespace RegComparator
{
    internal interface IUserReporter
    {
        public void ReportInfo(object sender, string message);
        public void ReportSpecial(object sender, string message);
        public void ReportError(object sender, string message);
    }
}
