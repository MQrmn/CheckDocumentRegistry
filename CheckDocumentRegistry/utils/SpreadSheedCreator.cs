

namespace CheckDocumentRegistry
{
    public class SpreadSheedCreator
    {
        public static void CreateSpreadSheets(DocumentsComparator documents, ProgramParameters arguments)
        {
            SpreadSheetWriterXLSX.Create(documents.Documents1CUppMatched, arguments.matchedUppPath);
            SpreadSheetWriterXLSX.Create(documents.Documents1CDoMatched, arguments.matchedDoPath);
            SpreadSheetWriterXLSX.Create(documents.documents1CUppSource, arguments.passedUppPath);
            SpreadSheetWriterXLSX.Create(documents.documents1CDoSource, arguments.passedDoPath);

        }

    }
}
