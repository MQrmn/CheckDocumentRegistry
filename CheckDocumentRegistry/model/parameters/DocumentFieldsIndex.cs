namespace RegComparator
{
    public class DocFieldsIndex
    {
        public int[] DocFielsdIndex1CDO = new int[] { 0, 3, 7, 6, 9, 8, 10, 11};          // Standard 1C:DO document format
        public int[] CustomDocFieldsIndex1CDO = new int[] { 0, 1, 3, 2, 5, 4, 6, 7};      // Simplified 1C:DO document format
        public int MaxPassedRowForSwitch1CDO = 8;
        public int RowLenght1CDO = 12;

        public int[] DocFieldsIndex1CUPP = new int[] { 0, 1, 4, 6, 3, 5, 7};              // Standard 1C:UPP document format
        public int[] CustomDocFieldsIndex1CUPP = new int[] { 0, 1, 3, 5, 2, 4, 6};        // Simplified 1C:UPP document format
        public int MaxPassedRowForSwitchUPP = 2;
        public int RowLenght1CUPP = 8;

        public int[] DocFieldsIndex1CKA = new int[] { 0, 0, 3, 4, 5, 6, 7};               // Standard 1C:KA document format
        public int[] CustomDocFieldsIndex1CKA = new int[] { 0, 0, 1, 2, 3, 4, 5 };        // Simplified 1C:KA document format
        public int MaxPassedRowForSwitch1CKA = 4;
        public int RowLenght1CKA = 8;

        public int[] docFieldsIndexCommon = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };         // Common document format
    }
}