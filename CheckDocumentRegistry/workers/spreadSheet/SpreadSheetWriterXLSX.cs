using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CheckDocumentRegistry
{
    internal class SpreadSheetWriterXLSX
    {
        public static void Create(List<Document> documents, string filePath)
        {

            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument
                .Create(filePath, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);

            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();

            Stylesheet stylesheet = workbookpart.WorkbookStylesPart.Stylesheet;
            List<UInt32> IndexRef = AddHeaderStyle(ref stylesheet);
            stylesheet.Save();

            workbookpart.Workbook = new Workbook();

            
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                AppendChild<Sheets>(new Sheets());

            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Sheet 1"
            };

            sheets.Append(sheet);

            Worksheet worksheet = worksheetPart.Worksheet;

            // Set wifth of column
            Columns lstColumns = worksheetPart.Worksheet.GetFirstChild<Columns>();
            Boolean needToInsertColumns = false;
            if (lstColumns == null)
            {
                lstColumns = new Columns();
                needToInsertColumns = true;
            }
            lstColumns.Append(new Column() { Min = 1, Max = 9, Width = 5, CustomWidth = true });  // Type
            lstColumns.Append(new Column() { Min = 2, Max = 9, Width = 60, CustomWidth = true }); // Title
            lstColumns.Append(new Column() { Min = 3, Max = 9, Width = 40, CustomWidth = true }); // ConterPart
            lstColumns.Append(new Column() { Min = 4, Max = 9, Width = 15, CustomWidth = true }); // Organiz
            lstColumns.Append(new Column() { Min = 5, Max = 9, Width = 15, CustomWidth = true }); // Date
            lstColumns.Append(new Column() { Min = 6, Max = 9, Width = 20, CustomWidth = true }); // Number
            lstColumns.Append(new Column() { Min = 7, Max = 9, Width = 15, CustomWidth = true }); // Summ
            lstColumns.Append(new Column() { Min = 8, Max = 9, Width = 10, CustomWidth = true }); // isUPD
            lstColumns.Append(new Column() { Min = 9, Max = 9, Width = 40, CustomWidth = true }); // Comment
            if (needToInsertColumns)
                worksheetPart.Worksheet.InsertAt(lstColumns, 0);

            SheetData sheetData = worksheet.GetFirstChild<SheetData>();

            string[] titleOfColumn = new string[9] {"Тип", "Наименование", "Контрагент", "Организация", "Дата", "Номер", "Сумма", "Является УПД", "Комментарий" };






            Row row = new Row();
            foreach (var i in titleOfColumn)
            {
                Cell cell = new Cell()
                {
                    CellValue = new CellValue(i.ToString()),
                    DataType = CellValues.String,
                    StyleIndex = IndexRef[2]

                };
                row.Append(cell);
            }

            sheetData.Append(row);

            documents.ForEach(delegate (Document document)
            {
                Row row = new Row();
                foreach (var i in document.GetArray())
                {
                    if (i is not null)
                    {
                        Cell cell = new Cell()
                        {
                            CellValue = new CellValue(i),
                            DataType = CellValues.String
                        };

                        row.Append(cell);
                    }
                }
                sheetData.Append(row);
            });

            Columns columns1 = worksheet.GetFirstChild<Columns>();

            workbookpart.Workbook.Save();
            spreadsheetDocument.Close();
        }

        private static List<UInt32> AddHeaderStyle(ref Stylesheet stylesheet)
        {
            UInt32 Fillid = 0, CellFormatid = 0;

            PatternFill pfill = new PatternFill() { PatternType = PatternValues.Solid };
            pfill.BackgroundColor = new BackgroundColor() { Rgb = HexBinaryValue.FromString("70AD47") };

            stylesheet.Fills.Append(new Fill() { PatternFill = pfill });
            Fillid = stylesheet.Fills.Count++;
            CellFormat cellFormat = new CellFormat() 
            { 
                FillId = Fillid,
                ApplyFill = true 
            };

            stylesheet.CellFormats.AppendChild(cellFormat);
            CellFormatid = stylesheet.CellFormats.Count++;

            return new List<uint>() { Fillid, CellFormatid };

        }
    }
}
