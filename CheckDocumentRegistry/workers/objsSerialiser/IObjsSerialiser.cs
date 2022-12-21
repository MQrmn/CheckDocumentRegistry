namespace RegComparator
{
    public interface IObjsSerialiser
    {
        public T? GetObj<T>(string filePathParams);
        public void PutObj(object obj, string filePath);
    }
}
