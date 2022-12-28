namespace RegComparator
{
    public interface ISpreadSheetWriterXLSX
    {
        public void CreateSpreadsheet(List<Document> documents, bool isDoDocument = true);

    }
}
