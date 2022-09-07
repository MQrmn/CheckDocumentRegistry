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

        const string filePath = @"C:\Users\alex_s\Desktop\1C\DocumentReport.xlsx";
        public void GetDocument(string filePath = filePath)
        {
           DoDocument[] doDocuments = new DoDocument[10];

           using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();

                

                // Перебираю листы
                
                foreach (Sheet thesheet in thesheetcollection.OfType<Sheet>())
                {
                    Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;

                    SheetData theSheetdata = theWorksheet.GetFirstChild<SheetData>();

                    var i = 0;
                    // Перебираю строки
                    for (int rCnt = 0; rCnt < theSheetdata.ChildElements.Count(); rCnt++)
                    {
                        var stringArr = this.GetParsedRow(workbookPart, theSheetdata, rCnt);
                        if (rCnt != 0)
                        {
                            Console.WriteLine(i + " ---------");
                            for (var j = 0; j < stringArr.Length; j++)
                            {
                                Console.WriteLine(stringArr[j]);
                            }

                            if (i++ > 10) break;
                            Console.WriteLine(" ---------");
                        }
                    }
                }
            }
        }


        string[] GetParsedRow(WorkbookPart workbookPart, SheetData theSheetdata, int rCnt)
        {
            

            string[] returedString = new string[11];

            // Перебираю ячейки в строке
            for (int rCnt1 = 0;
                rCnt1 < theSheetdata.ElementAt(rCnt).ChildElements.Count()
                ; rCnt1++)
            {
                Cell thecurrentcell = (Cell)theSheetdata.ElementAt(rCnt).ChildElements.ElementAt(rCnt1);

                string currentcellvalue = string.Empty;
                if (thecurrentcell.DataType != null)
                {
                    if (thecurrentcell.DataType == CellValues.SharedString)
                    {

                        int id;
                        if (Int32.TryParse(thecurrentcell.InnerText, out id))
                        {
                            SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                            if (item.Text != null)
                            {
                                //Console.WriteLine(item.Text.Text);
                                returedString[rCnt1] = item.Text.Text;
                            }
                        }
                    }
                    else if (thecurrentcell.DataType == CellValues.Number)
                    {
                        //Console.WriteLine(thecurrentcell.InnerText.GetType());
                        returedString[rCnt1] = thecurrentcell.InnerText;
                    }
                        

                    else if (thecurrentcell.DataType == CellValues.Error)
                        returedString[rCnt1]  = null;
                }
            }

            DoDocument doDocument = new DoDocument(returedString);

            return returedString;
        }
    }
}
