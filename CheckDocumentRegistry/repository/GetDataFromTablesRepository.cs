using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DocumentsComparator
{
    public class GetDataFromTablesRepository
    {
        const string filePath = @"C:\1C\DocumentReport.xlsx";
        public string[][] GetDocumentsFromTable(string filePath = filePath)
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();

                foreach (Sheet thesheet in thesheetcollection.OfType<Sheet>())
                {
                    Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;
                    SheetData theSheetdata = theWorksheet.GetFirstChild<SheetData>();

                    int rowNumber = theSheetdata.ChildElements.Count();
                    string[][] allDocuments = GetDocumentsArray(workbookPart, theSheetdata, rowNumber);
                    
                    return allDocuments;
                }

            }

            string[][] emptyDocuments = new string[1][] { new string[] { null } } ;
            return emptyDocuments;
        }

        string[][] GetDocumentsArray(WorkbookPart workbookPart, SheetData theSheetdata, int rowNumber)
        {
            string[][] allDocuments = new string[rowNumber][];
            int i = 0;

            for (int rowCount = 0; rowCount < rowNumber; rowCount++)
            {
                string[] docValues = this.GetParsedRow(workbookPart, theSheetdata, rowCount);
                allDocuments[rowCount] = docValues;
                //break;
            }

            return allDocuments;
        }

        string[] GetParsedRow(WorkbookPart workbookPart, SheetData theSheetdata, int rCnt)
        {
            int cellNumber = theSheetdata.ElementAt(rCnt).ChildElements.Count();
            string[] returedString = new string[cellNumber];

            // Перебираю ячейки в строке
            for (int cellCount = 0;
                cellCount < theSheetdata.ElementAt(rCnt).ChildElements.Count()
                ; cellCount++)
            {
                Cell thecurrentcell = (Cell)theSheetdata.ElementAt(rCnt).ChildElements.ElementAt(cellCount);

                //Console.WriteLine("1 " + thecurrentcell.InnerText);
                //string currentcellvalue = string.Empty;
                if (thecurrentcell.DataType != null)
                {
                    //Console.WriteLine("2 " + thecurrentcell.InnerText.GetType());
                    if (thecurrentcell.DataType == CellValues.SharedString)
                    {

                        int id;
                        if (Int32.TryParse(thecurrentcell.InnerText, out id))
                        {
                            SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                            if (item.Text != null)
                            {
                                //Console.WriteLine(item.Text.Text);
                                returedString[cellCount] = item.Text.Text;
                            }
                        }
                    }

                    else if (thecurrentcell.DataType == CellValues.Number)
                    {
                        //Console.WriteLine(thecurrentcell.InnerText.GetType());
                        returedString[cellCount] = thecurrentcell.InnerText;
                    }

                    else if (thecurrentcell.DataType == CellValues.Error)
                        returedString[cellCount] = null;
                    //Console.WriteLine(thecurrentcell.InnerText.GetType());
   
                }
                else
                    returedString[cellCount] = thecurrentcell.InnerText;
            }

            return returedString;

        }
    }
}
