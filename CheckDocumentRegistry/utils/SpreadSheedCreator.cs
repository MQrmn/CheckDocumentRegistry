

namespace CheckDocumentRegistry
{
    public class SpreadSheedCreator
    {
        public static void CreateSpreadSheets(DocumentsComparator documents, ProgramParameters arguments)
        {
            SpreadSheetWriterXLSX.Create(documents.matchedUppDocuments, arguments.matchedUppPath);
            SpreadSheetWriterXLSX.Create(documents.matchedDoDocuments, arguments.matchedDoPath);
            SpreadSheetWriterXLSX.Create(documents.uppDocuments, arguments.passedUppPath);
            SpreadSheetWriterXLSX.Create(documents.doDocuments, arguments.passedDoPath);

        }

    }
}
