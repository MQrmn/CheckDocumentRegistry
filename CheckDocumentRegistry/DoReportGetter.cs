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
        
        



        //const string filePath = @"C:\1C\DocumentReport.xlsx";
        //public void GetDocument(string filePath = filePath)
        //{
        //   DraftDoDocument[] doDocuments = new DraftDoDocument[10];

        //   using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
        //    {
        //        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
        //        Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();

        //        // Перебираю листы
        //        foreach (Sheet thesheet in thesheetcollection.OfType<Sheet>())
        //        {
        //            Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;
        //            SheetData theSheetdata = theWorksheet.GetFirstChild<SheetData>();

        //            var i = 0;
        //            // Перебираю строки
        //            for (int rCnt = 0; rCnt < theSheetdata.ChildElements.Count(); rCnt++)
        //            {
        //                if (rCnt != 0)
        //                {
        //                    string[] docValues = this._GetParsedRow(workbookPart, theSheetdata, rCnt);
        //                    DoDocument doDocument = new DoDocument(docValues);
        //                    Console.WriteLine("--------");
        //                    Console.WriteLine(doDocument.docTitle);
        //                    Console.WriteLine(doDocument.docType);
        //                    Console.WriteLine(doDocument.docCounterparty);
        //                    Console.WriteLine(doDocument.docNumber);
        //                    Console.WriteLine(doDocument.docDate);
        //                    Console.WriteLine(doDocument.docSum);
        //                    Console.WriteLine(doDocument.docCompany);
        //                    Console.WriteLine(doDocument.isUpd);
        //                }
        //                if (i++ > 1) break;

        //            }
        //        }
        //    }
        //}


        //string[] _GetParsedRow(WorkbookPart workbookPart, SheetData theSheetdata, int rCnt)
        //{

        //    string[] returedString = new string[8];

        //    // Перебираю ячейки в строке
        //    for (int rCnt1 = 0;
        //        rCnt1 < theSheetdata.ElementAt(rCnt).ChildElements.Count()
        //        ; rCnt1++)
        //    {
        //        Cell thecurrentcell = (Cell)theSheetdata.ElementAt(rCnt).ChildElements.ElementAt(rCnt1);

        //        //string currentcellvalue = string.Empty;
        //        if (thecurrentcell.DataType != null)
        //        {
        //            if (thecurrentcell.DataType == CellValues.SharedString)
        //            {

        //                int id;
        //                if (Int32.TryParse(thecurrentcell.InnerText, out id))
        //                {
        //                    SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        //                    if (item.Text != null)
        //                    {
        //                        //Console.WriteLine(item.Text.Text);
        //                        returedString[rCnt1] = item.Text.Text;
        //                    }
        //                }
        //            }
        //            else if (thecurrentcell.DataType == CellValues.Number)
        //            {
        //                //Console.WriteLine(thecurrentcell.InnerText.GetType());
        //                returedString[rCnt1] = thecurrentcell.InnerText;
        //            }


        //            else if (thecurrentcell.DataType == CellValues.Error)
        //                returedString[rCnt1]  = null;
        //        }
        //    }

        //    return returedString;
        //}
    }
}
