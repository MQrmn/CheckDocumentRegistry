namespace RegComparator
{
    public interface ISpreadSheetReader
    {
        public string[][] GetDocumentsFromTable(string spreadsheetPath);
    }
}
