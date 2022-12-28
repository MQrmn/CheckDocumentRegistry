namespace RegComparator
{
    internal class DocumentAmountReporter
    {
        //private string reportFilePath;

        private DocAmountsRepositoryBase _docAmounts;
        private DocRepositoryBase _docRepository;


        internal DocumentAmountReporter(DocAmountsRepositoryBase docAmounts, DocRepositoryBase docRepository)
        {
            _docAmounts = docAmounts;
            _docRepository = docRepository;
        }

        internal void CreateAllReports()
        {
            string[] docAmounts = GetReportDataCommon();
            List<string> companiesList = GetCompanies(_docRepository.UnmatchedDocs);
            string[] compiledReportEntries = GetReportDataByCompanies(_docRepository.UnmatchedDocs, companiesList);
            string[] arrayReportData = docAmounts.Concat(compiledReportEntries).ToArray();
            string stringReportData = GetStringFromArr(arrayReportData);
            //this.PutReportConsole(stringReportData);
        }

        private void PutReport()
        {

        }

        private string[] GetReportDataCommon()
        {
            string[] commonReportData = new string[10];

            commonReportData[0] = "Результат сравнения документов, внесенных в 1С:ДО, с реестром\n";
            commonReportData[1] = "От " + DateTime.Now.ToLongDateString() + ":\n\n";
            commonReportData[2] = "Документов в 1С:ДО Source: " + _docAmounts.Amounts1CDO.Source + "\n";
            commonReportData[3] = "Документов в реестре Source: " + _docAmounts.AmountsReg.Source + "\n";
            commonReportData[4] = "Документов 1С:ДО Skipped: " + _docAmounts.Amounts1CDO.Skip + "\n";
            commonReportData[5] = "Документов реестра Skipped: " + _docAmounts.AmountsReg.Skip + "\n";
            commonReportData[6] = "Документов 1С:ДО Matched:  " + _docAmounts.Amounts1CDO.Matched + "\n";
            commonReportData[7] = "Документов реестра Matched: " + _docAmounts.AmountsReg.Matched + "\n";
            commonReportData[8] = "Документов 1С:ДО Unmatched: " + _docAmounts.Amounts1CDO.Unmatched + "\n";
            commonReportData[9] = "Документов Registry Unmatched: " + _docAmounts.AmountsReg.Unmatched + "\n\n";

            return commonReportData;
        }

        private string[] GetReportDataByCompanies(List<Document> documents, List<string> companies)
        {
            string[] byCompaniesreportData = new string[companies.Count + 1];
            int listPosition = 0;
            byCompaniesreportData[listPosition] = "Количество не внесенных документов по организациям согласно реестру :\n\n";

            foreach (var company in companies)
            {
                listPosition++;
                List<Document> matchedDocuments = documents.FindAll(delegate (Document document)
                {
                    if (document.Company == company) return true;
                    return false;
                });
                byCompaniesreportData[listPosition] = company + ": " + matchedDocuments.Count + "\n";
            }
            return byCompaniesreportData;
        }


        private List<string> GetCompanies(List<Document> documents)
        {
            List<string> companies = new();
            foreach (var document in documents)
            {
                bool isCompanyExist = companies.Contains(document.Company);
                if (!isCompanyExist) companies.Add(document.Company);
            }
            return companies;
        }

        private string GetStringFromArr(string[] arrayData)
        {
            string stringData = String.Empty;
            foreach (var i in arrayData)
            {
                stringData += i;
            }
            return stringData;
        }
    }
}
