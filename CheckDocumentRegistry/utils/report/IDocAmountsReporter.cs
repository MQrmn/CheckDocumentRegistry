namespace RegComparator
{
    public interface IDocAmountsReporter
    {
        public event EventHandler<string>? Notify;
        public void CreateReport();
    }
}
