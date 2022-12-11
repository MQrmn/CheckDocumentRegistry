
namespace RegComparator
{
    public abstract class DocFieldsBase
    {
        public int[] DocFielsdIndex;
        public int RowLenght;
        public int MaxPassedRows;

        public DocFieldsBase()
        {
            SetDefaults();
        }

        public DocFieldsBase(int[] docFielsdIndex,
                             int rowLenght,
                             int maxPassedRows)
        {
            DocFielsdIndex = docFielsdIndex;
            RowLenght = rowLenght;
            MaxPassedRows = maxPassedRows;
        }

        public abstract void SetDefaults();
    }
}
