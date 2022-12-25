namespace RegComparator
{
    internal interface IDocLoader
    {
        public void GetDocObjectList(
                                string[] spreadsheetPathArr,
                                Action<string[], int[]> addDocumentAction,
                                DocFieldsBase fieldsSettings
                                );
    }
}
