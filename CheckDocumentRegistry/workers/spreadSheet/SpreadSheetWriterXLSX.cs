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
            workbookpart.Workbook = new Workbook();
            worksheetPart.Worksheet = new Worksheet(new SheetData());
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                AppendChild<Sheets>(new Sheets());


            WorkbookStylesPart stylePart = workbookpart.AddNewPart<WorkbookStylesPart>();
            stylePart.Stylesheet = GenerateStylesheet();
            stylePart.Stylesheet.Save();

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
            lstColumns = new Columns();
            lstColumns.Append(new Column() { Min = 1, Max = 9, Width = 5, CustomWidth = true, Hidden = true });  // Type
            lstColumns.Append(new Column() { Min = 2, Max = 9, Width = 60, CustomWidth = true }); // Title
            lstColumns.Append(new Column() { Min = 3, Max = 9, Width = 40, CustomWidth = true }); // ConterPart
            lstColumns.Append(new Column() { Min = 4, Max = 9, Width = 15, CustomWidth = true }); // Organiz
            lstColumns.Append(new Column() { Min = 5, Max = 9, Width = 15, CustomWidth = true }); // Date
            lstColumns.Append(new Column() { Min = 6, Max = 9, Width = 20, CustomWidth = true }); // Number
            lstColumns.Append(new Column() { Min = 7, Max = 9, Width = 15, CustomWidth = true }); // Summ
            lstColumns.Append(new Column() { Min = 8, Max = 9, Width = 10, CustomWidth = true }); // isUPD
            lstColumns.Append(new Column() { Min = 9, Max = 9, Width = 40, CustomWidth = true }); // Comment
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
                    StyleIndex = 1
                };
                row.Append(cell);
            }

            sheetData.Append(row);


           


            documents.ForEach(delegate (Document document)
            {
                string[] documentInArray = document.GetArray();
                Row row = new Row();

                for (var i = 0; i < documentInArray.Length; i++ )
                {
                    if (documentInArray[i] is not null)
                    {
                        Cell cell = new Cell()
                        {
                            CellValue = new CellValue(documentInArray[i]),
                            DataType = CellValues.String,
                            StyleIndex = GetStyleIndex(document, i)
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

        private static uint GetStyleIndex(Document document, int position)
        {
            uint styleIndex = 0;
            int stylePosition = document.StylePosition;

            if (stylePosition == 0)
                styleIndex = 3;

            if (stylePosition != 0 && stylePosition == position )
                styleIndex = 2;

            return styleIndex;
        }

        private static Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;



            Fonts fonts =   new Fonts(
                new Font(new FontSize() { Val = 10 }),
                            new Font(new FontSize() { Val = 10 }, new Bold(), new Color() { Rgb = "FFFFFF" } ));

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                                new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                                new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } }) // Index 2 - Header
                                { PatternType = PatternValues.Solid }),
                                new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "fcf803" } }) // Index 3 - Body hilight
                                { PatternType = PatternValues.Solid }),
                                new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "a9acb0" } }) // Index 3 - Body hilight
                                { PatternType = PatternValues.Solid })
                );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat { FontId = 1, FillId = 2, BorderId = 0, ApplyFill = true,
                        Alignment = new Alignment()
                        {
                            Horizontal = HorizontalAlignmentValues.Center
                        }}, // header
                    new CellFormat { FontId = 0, FillId = 3, BorderId = 0, ApplyFill = true }, // body 
                    new CellFormat { FontId = 0, FillId = 4, BorderId = 0, ApplyFill = true }  // body 
                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }

    }
}
