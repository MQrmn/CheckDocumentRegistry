namespace RegComparator
{
    public interface IObjsSerialiser
    {
        public T? GetObj<T>(string filePathParams);
    }
}
