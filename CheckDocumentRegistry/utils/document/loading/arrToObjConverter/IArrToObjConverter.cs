namespace RegComparator
{
    public interface IArrToObjConverter
    {
        public event EventHandler<string>? ErrNotify;
        public void ConvertArrToObjs(string[][] docsArr, string? passedDocsReportPath);
    }
}
