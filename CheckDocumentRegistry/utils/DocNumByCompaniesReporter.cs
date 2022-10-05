using DocumentFormat.OpenXml.Office.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDocumentRegistry
{
    internal class DocNumByCompaniesReporter
    {
        internal void PrintReport(List<Document> documents)
        {

            List<string> companies = this.GetCompanies(documents);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nКоличество не внесенных документов по контрагентам согласно 1С:УПП :");

            foreach (var company in companies)
            {
                List<Document> matchedDocuments = documents.FindAll(delegate (Document document)
                {
                    if (document.Company == company) return true;
                    return false;
                });
                
                Console.WriteLine(company + ": " + matchedDocuments.Count);
            }

            Console.ResetColor();
            Console.WriteLine();

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
