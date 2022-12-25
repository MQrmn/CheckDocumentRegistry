namespace RegComparator
{
    public interface IObjsConverter
    {
        public event EventHandler<string>? ErrNotify;
        public T? GetObj<T>(string filePathParams);
        public void PutObj(object obj, string filePath);
    }
}
