using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DocumentsComparator
{
    public class DoReportGetter
    {


        public void GetDocument()
        {
            GetDataFromTablesRepository getDataFromTablesRepository = new GetDataFromTablesRepository();
            string[][] documentsData = getDataFromTablesRepository.GetDocumentsFromTable();

            foreach (var i in documentsData)
                foreach (var j in i)
                    Console.WriteLine("- " + j);

        }
       
    }
}
