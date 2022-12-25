namespace RegComparator
{
    public interface IFileExistChecker
    {
        public event EventHandler<string>? ErrNotify;
        public void CheckCritical(string[] paths);
        public void CheckNonCritical(string[] paths);
    }
}
