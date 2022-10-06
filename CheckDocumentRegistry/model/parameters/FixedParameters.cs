
namespace CheckDocumentRegistry
{
    internal class FixedParameters
    {
        internal int[] DocFieldIndex1CDo;            // Standard 1C:DO document format
        internal int[] DustomDocFieldIndex1CDo;      // Simplified 1C:DO document format
        internal int[] DocFieldIndex1CUpp;           // Standard 1C:UPP document format
        internal int[] DustomDocFieldIndex1CUpp;     // Simplified 1C:UPP document format
        readonly internal string ProgramParametersFilePath;
        readonly internal string LastCompareDocumentsAnountsFilePath;

        internal FixedParameters()
        {
            this.DocFieldIndex1CDo = new int[] { 0, 3, 6, 7, 8, 9, 10, 11 };
            this.DustomDocFieldIndex1CDo = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            this.DocFieldIndex1CUpp = new int[] { 0, 1, 3, 4, 5, 6, 7 };
            this.DustomDocFieldIndex1CUpp = new int[] { 0, 1, 2, 3, 4, 5, 6 };
            this.ProgramParametersFilePath = "params.json";
            this.LastCompareDocumentsAnountsFilePath = "lastCompareAmounts.json";
        }

    }
}
