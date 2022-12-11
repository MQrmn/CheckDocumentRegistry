namespace RegComparator
{
    internal class DocFieldsCommon : DocFieldsBase
    {
        public override void SetDefaults()
        {
            DocFielsdIndex = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            RowLenght = 8;
            MaxPassedRows = 1;
        }
    }
}
