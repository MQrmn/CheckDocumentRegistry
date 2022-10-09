namespace CheckDocumentRegistry
{
    internal class DocFieldsIndex
    {
        internal int[] DocFielsdIndex1CDO = new int[] { 0, 3, 7, 6, 9, 8, 10, 11};          // Standard 1C:DO document format
        internal int[] CustomDocFieldsIndex1CDO = new int[] { 0, 1, 3, 2, 5, 4, 6, 7};      // Simplified 1C:DO document format
        internal int maxPassedRowForSwitch1CDO = 8;
        internal int rowLenght1CDO = 12;

        internal int[] DocFieldsIndex1CUPP = new int[] { 0, 1, 4, 6, 3, 5, 7};              // Standard 1C:UPP document format
        internal int[] CustomDocFieldsIndex1CUPP = new int[] { 0, 1, 3, 5, 2, 4, 6};        // Simplified 1C:UPP document format
        internal int maxPassedRowForSwitchUPP = 1;
        internal int rowLenght1CUPP = 8;

        internal int[] docFieldsIndexUniversal = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };      // Universal document format
    }
}
