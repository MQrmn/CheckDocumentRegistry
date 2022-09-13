

namespace CheckDocumentRegistry
{
    public class ReportCreator
    {
        public static void CreateReports(Comparator documents, Arguments arguments)
        {
            ReportRepository.Create(documents.matchedUppDocuments, arguments.matchedUppPath);
            ReportRepository.Create(documents.matchedDoDocuments, arguments.matchedDoPath);
            ReportRepository.Create(documents.uppDocuments, arguments.unMatchedUppPath);
            ReportRepository.Create(documents.doDocuments, arguments.unMatchedDoPath);

        }

    }
}
