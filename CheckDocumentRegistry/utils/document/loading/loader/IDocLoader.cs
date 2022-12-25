namespace RegComparator
{
    public interface IDocLoader
    {
        public event EventHandler<string>? Notify;
        public void GetDocObjectList(
                                string[] spreadsheetPathArr,
                                Action<string[], int[]> addDocumentAction,
                                DocFieldsBase fieldsSettings
                                );
    }
}
