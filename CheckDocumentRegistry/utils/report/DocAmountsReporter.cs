namespace RegComparator
{
    public class DocAmountsReporter : IDocAmountsReporter
    {
        private DocAmountsRepositoryBase _docAmounts;
        private DocRepositoryBase _docRepository;

        public event EventHandler<string>? Notify;

        public DocAmountsReporter(DocAmountsRepositoryBase docAmounts, DocRepositoryBase docRepository)
        {
            _docAmounts = docAmounts;
            _docRepository = docRepository;
        }

        public void CreateReport()
        {
            string[] docAmounts = GetCommonAmounts();
            List<string> companiesList = GetCompaniesList(_docRepository.UnmatchedDocs);
            string[] docsByCompanies = GetDocAmountsByCompanies(_docRepository.UnmatchedDocs, companiesList);
            string[] arrayReportData = docAmounts.Concat(docsByCompanies).ToArray();
            PutReport(arrayReportData);
        }

        private void PutReport(string[] reportStrings)
        {
            foreach (var str in reportStrings)
            {
                Notify?.Invoke(this, str);
            }
        }

        private string[] GetCommonAmounts()
        {
            string[] commonReportData = new string[10];

            commonReportData[0] = "\nРезультат сравнения документов";
            commonReportData[1] = "От " + DateTime.Now.ToLongDateString() + ":\n";
            commonReportData[2] = "1С:ДО исходное кол-во: " + _docAmounts.Amounts1CDO.Source;
            commonReportData[3] = "Реестр исходное кол-во: " + _docAmounts.AmountsReg.Source;
            commonReportData[4] = "1С:ДО игнор-лист: " + _docAmounts.Amounts1CDO.Skip;
            commonReportData[5] = "Реестр игнор-лист: " + _docAmounts.AmountsReg.Skip;
            commonReportData[6] = "1С:ДО совпавших:  " + _docAmounts.Amounts1CDO.Matched;
            commonReportData[7] = "Реестр совпавших: " + _docAmounts.AmountsReg.Matched;
            commonReportData[8] = "1С:ДО не совпавших: " + _docAmounts.Amounts1CDO.Unmatched;
            commonReportData[9] = "Реестр не совпавших: " + _docAmounts.AmountsReg.Unmatched + "\n";

            return commonReportData;
        }

        private string[] GetDocAmountsByCompanies(List<Document> documents, List<string> companies)
        {
            string[] byCompaniesreportData = new string[companies.Count + 1];
            int listPosition = 0;
            byCompaniesreportData[listPosition] = "Не внесенных документов по организациям:\n";

            foreach (var company in companies)
            {
                listPosition++;
                List<Document> matchedDocuments = documents.FindAll(delegate (Document document)
                {
                    if (document.Company == company) return true;
                    return false;
                });
                byCompaniesreportData[listPosition] = company + ": " + matchedDocuments.Count;
            }
            return byCompaniesreportData;
        }

        private List<string> GetCompaniesList(List<Document> documents)
        {
            List<string> companies = new();
            foreach (var document in documents)
            {
                bool isCompanyExist = companies.Contains(document.Company);
                if (!isCompanyExist) companies.Add(document.Company);
            }
            return companies;
        }
    }
}
