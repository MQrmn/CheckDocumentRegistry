using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;

namespace CheckDocumentRegistry
{
    public class SpreadSheetReaderXLSX
    {
        
        public string[][] GetDocumentsFromTable(string filePath)
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                Sheets sheetCollection = workbookPart.Workbook.GetFirstChild<Sheets>();

                foreach (Sheet sheet in sheetCollection.OfType<Sheet>())
                {
                    Worksheet workSheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();

                    int rowNumber = sheetData.ChildElements.Count();
                    string[][] allDocuments = GetDocumentsArray(workbookPart, sheetData, rowNumber);
                    
                    return allDocuments;
                }
            }
            string[][] emptyDocuments = new string[1][];
            return emptyDocuments;
        }


        string[][] GetDocumentsArray(WorkbookPart workbookPart, SheetData sheetData, int rowNumber)
        {
            string[][] allDocuments = new string[rowNumber][];

            for (int rowCount = 0; rowCount < rowNumber; rowCount++)
            {
                string[] docValues = this.GetParsedRow(workbookPart, sheetData, rowCount);
                allDocuments[rowCount] = docValues;
            }

            return allDocuments;
        }


        string[] GetParsedRow(WorkbookPart workbookPart, SheetData sheetData, int rowCount)
        {
            int cellNumber = sheetData.ElementAt(rowCount).ChildElements.Count();
            string[] parsedRow = new string[cellNumber];

            // Going through the cells in the row
            for (
                int cellCount = 0;
                cellCount < sheetData.ElementAt(rowCount).ChildElements.Count(); 
                cellCount++
                )
            {
                Cell currentCell = (Cell)sheetData.ElementAt(rowCount).ChildElements.ElementAt(cellCount);


                if (currentCell.DataType == null)
                {
                    if (cellCount == 4)
                        parsedRow[cellCount] = Regex.Replace(currentCell.InnerText, @"\.0", String.Empty);
                    else
                        parsedRow[cellCount] = currentCell.InnerText;
                    continue;
                }

                if (currentCell.DataType == CellValues.SharedString)
                {
                    int id = Int32.Parse(currentCell.InnerText);
                    SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                    parsedRow[cellCount] = item.Text.Text;
                }

                else if (currentCell.DataType == CellValues.Number)
                {
                    parsedRow[cellCount] = currentCell.InnerText;
                    
                }

                else if (currentCell.DataType == CellValues.Error)
                    parsedRow[cellCount] = null;
            }

            return parsedRow;

        }
    }
}
