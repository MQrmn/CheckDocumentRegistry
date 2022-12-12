namespace RegComparator
{
    internal class DocumentAmountReporter
    {
        private string reportFilePath;

        internal DocumentAmountReporter(string filePath)
        {
            this.reportFilePath = filePath;
        }

        internal void CreateAllReports(List<Document> docList, DocAmountReportData documentsAmount)
        {
            string[] docAmounts = GetReportDataCommon(documentsAmount);
            List<string> companiesList = GetCompanies(docList);
            string[] compiledReportEntries = GetReportDataByCompanies(docList, companiesList);
            string[] arrayReportData = docAmounts.Concat(compiledReportEntries).ToArray();
            string stringReportData = GetStringFromArr(arrayReportData);
            this.PutReportConsole(stringReportData);
            this.PutReportTXT(this.reportFilePath, stringReportData);
        }

        private void PutReportConsole(string reportData)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine(reportData);
            Console.ResetColor();
        }

        private void PutReportTXT(string reportFilePath, string reportData)
        {
            try
            {
                File.WriteAllText(reportFilePath, reportData);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Не удалось записать файл: " + reportFilePath);
                Console.ResetColor();
            }
        }

        private string[] GetReportDataCommon(DocAmountReportData documentsAmount)
        {
            string[] commonReportData = new string[10];

            commonReportData[0] = "Результат сравнения документов, внесенных в 1С:ДО, с реестром\n";
            commonReportData[1] = "От " + DateTime.Now.ToLongDateString() + ":\n\n";
            commonReportData[2] = "Документов в 1С:ДО всего: " + documentsAmount.doDocumentsCount + "\n";
            commonReportData[3] = "Документов в реестре всего: " + documentsAmount.uppDocumentsCount + "\n";
            commonReportData[4] = "Документов 1С:ДО не внесенных в реестр: " + documentsAmount.ignoreDoDocumentsCount + "\n";
            commonReportData[5] = "Документов реестра исключено из проверки: " + documentsAmount.ignoreUppDocumentsCount + "\n";
            commonReportData[6] = "Документов 1С:ДО совпали с реестром:  " + documentsAmount.Documents1CDoMatchedCount + "\n";
            commonReportData[7] = "Документов реестра совпали с 1С:ДО: " + documentsAmount.Documents1CUppMatchedCount + "\n";
            commonReportData[8] = "Документов 1С:ДО внесенных с ошибкой: " + documentsAmount.Documents1CDoUnmatchedCount + "\n";
            commonReportData[9] = "Документов осталось внести: " + documentsAmount.Documents1CUppUnmatchedCount + "\n\n";

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
