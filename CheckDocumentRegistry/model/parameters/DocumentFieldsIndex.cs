using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDocumentRegistry
{
    internal class DocumentFieldsIndex
    {
        internal int[] DocFieldIndex1CDo = new int[] { 0, 3, 6, 7, 8, 9, 10, 11 };          // Standard 1C:DO document format
        internal int[] DustomDocFieldIndex1CDo = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };      // Simplified 1C:DO document format
        internal int maxPassedRowForSwitchDo = 8;
        internal int rowLenghtDo = 12;

        internal int[] DocFieldIndex1CUpp = new int[] { 0, 1, 3, 4, 5, 6, 7 };              // Standard 1C:UPP document format
        internal int[] DustomDocFieldIndex1CUpp = new int[] { 0, 1, 2, 3, 4, 5, 6 };        // Simplified 1C:UPP document format
        internal int maxPassedRowForSwitchUpp = 1;
        internal int rowLenghtUpp = 8;

    }
}
