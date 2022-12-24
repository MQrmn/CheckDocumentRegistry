namespace RegComparator
{
    public interface IFileExistChecker
    {
        public void CheckCritical(string[] paths);
        public void CheckNonCritical(string[] paths);
    }
}
