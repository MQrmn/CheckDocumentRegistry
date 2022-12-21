﻿namespace RegComparator
{
    public interface IObjsConverter
    {
        public T? GetObj<T>(string filePathParams);
        public void PutObj(object obj, string filePath);
    }
}
