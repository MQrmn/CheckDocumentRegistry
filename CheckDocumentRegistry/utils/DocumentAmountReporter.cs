
namespace CheckDocumentRegistry
{
    internal class DocumentAmountReporter
    {
        string reportFilePath;

        internal DocumentAmountReporter(string filePath)
        {
            this.reportFilePath = filePath;
        }


        internal void CreateAllReports(List<Document> documents, DocumentsAmount documentsAmount)
        {
            string[] commonReportData = GetReportDataCommon(documentsAmount);
            List<string> companies = GetCompanies(documents);
            string[] byCompaniesReportData = GetReportDataByCompanies(documents, companies);
            string[] reportData = commonReportData.Concat(byCompaniesReportData).ToArray();

            this.PutReportConsole(reportData);
            this.PutReportTXT(this.reportFilePath, reportData);
        }


        internal void PutReportConsole(string[] reportData)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            foreach (var i in reportData)
            {
                Console.WriteLine(i);
            }
            Console.ResetColor();
        }


        internal void PutReportTXT(string reportFilePath, string[] reportData)
        {
                File.Delete(reportFilePath);
                File.AppendAllLines(reportFilePath, reportData);
        }


        internal string[] GetReportDataCommon(DocumentsAmount documentsAmount)
        {

            string[] commonReportData = new string[10];

            commonReportData[0] = "Результат сравнения документов, внесеннных в 1С:ДО, с реестром 1С:УПП";
            commonReportData[1] = "От " + DateTime.Now.ToLongDateString() + ":\n";
            commonReportData[2] = "Документов в 1С:ДО всего: " + documentsAmount.doDocumentsCount;
            commonReportData[3] = "Документов в реестре всего: " + documentsAmount.uppDocumentsCount;
            commonReportData[4] = "Документов 1С:ДО не внесенных в реестр: " + documentsAmount.ignoreDoDocumentsCount;
            commonReportData[5] = "Документов реестра исключено из проверки: " + documentsAmount.ignoreUppDocumentsCount;
            commonReportData[6] = "Документов 1С:ДО совпали с реестром:  " + documentsAmount.Documents1CDoMatchedCount;
            commonReportData[7] = "Документов реестра совпали с 1С:ДО: " + documentsAmount.Documents1CUppMatchedCount;
            commonReportData[8] = "Документов 1С:ДО внесенных с ошибкой: " + documentsAmount.Documents1CDoUnmatchedCount;
            commonReportData[9] = "Документов осталось внести: " + documentsAmount.Documents1CUppUnmatchedCount;

            return commonReportData;
        }


        internal string[] GetReportDataByCompanies(List<Document> documents, List<string> companies)
        {
            string[] byCompaniesreportData = new string[companies.Count + 1];
            int listPosition = 0;
            byCompaniesreportData[listPosition] = "\nКоличество не внесенных документов по организациям согласно 1С:УПП :";

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


        internal List<string> GetCompanies(List<Document> documents)
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
