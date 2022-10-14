using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;


namespace RegComparator
{
    public class SpreadSheetWriterXLSX
    {
        internal protected SpreadsheetDocument spreadsheetDocument;
        internal protected WorkbookPart workbookpart;
        internal protected WorksheetPart worksheetPart;
        internal protected Worksheet worksheet;

        internal protected SpreadSheetWriterXLSX(string filePath)
        {
            this.spreadsheetDocument = SpreadsheetDocument
                .Create(filePath, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);
            this.workbookpart = spreadsheetDocument.AddWorkbookPart();
            this.worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            workbookpart.Workbook = new Workbook();
            this.worksheetPart.Worksheet = new Worksheet(new SheetData());

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
            this.worksheet = worksheetPart.Worksheet;
        }

        internal protected void CreateSpreadsheet(List<Document> documents, bool isDoDocument = true)
        {
            // Setting columns
            SetColumns(ref this.worksheetPart);
            SheetData sheetData = this.worksheet.GetFirstChild<SheetData>();

            // Filling header row
            string[] titlesOfColumns = new string[9] {"Тип", "Наименование", "Контрагент", 
                                                    "Организация", "Дата", "Номер", 
                                                    "Сумма", "Является УПД", "Комментарий" };

            Row headerRow = GetHeaderRow(titlesOfColumns);
            sheetData.Append(headerRow);


            // Filling body
            documents.ForEach(delegate (Document document)
            {
                Row row = GetRow(document, isDoDocument);
                sheetData.Append(row);
            });

            this.workbookpart.Workbook.Save();
            this.spreadsheetDocument.Close();
        }


        internal protected void CreateSpreadsheet(List<string[]> documents)
        {
            // Setting columns
            SheetData sheetData = this.worksheet.GetFirstChild<SheetData>();

            // Filling body
            documents.ForEach(delegate (string[] document)
            {
                Row row = GetRow(document);
                sheetData.Append(row);
            });

            this.workbookpart.Workbook.Save();
            this.spreadsheetDocument.Close();
        }


        // Getting slyle index by document data
        internal protected uint GetStyleIndex(Document document, int currentPosition, bool isDoDocument)
        {
            if (!isDoDocument) return 0;

            uint styleIndex = 0;
            int stylePosition = document.StylePosition;

            if (stylePosition == 0)
                styleIndex = 3;

            if (stylePosition != 0 && stylePosition == currentPosition )
                styleIndex = 2;

            return styleIndex;
        }

        // Filling body
        internal protected Row GetRow( Document document, bool isDoDocument)
        {
            Row row = new Row();

            string[] documentInArray = document.GetArray();

            for (var i = 0; i < documentInArray.Length; i++)
            {
                if (documentInArray[i] is not null)
                {
                    Cell cell = new Cell()
                    {
                        CellValue = new CellValue(documentInArray[i]),
                        DataType = CellValues.String,
                        StyleIndex = GetStyleIndex(document, i, isDoDocument)
                    };

                    row.Append(cell);
                }
            }

            return row;
        }

        internal protected Row GetRow(string[] document)
        {
            Row row = new Row();

            for (var i = 0; i < document.Length; i++)
            {
                if (document[i] is not null)
                {
                    Cell cell = new Cell()
                    {
                        CellValue = new CellValue(document[i]),
                        DataType = CellValues.String,
                    };

                    row.Append(cell);
                }
            }

            return row;
        }


        // Filling header
        internal protected Row GetHeaderRow(string[] titleOfColumn)
        {
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

            return row;
        }

        // Setting columns
        internal protected void SetColumns(ref WorksheetPart worksheetPart)
        {
            Columns columns = worksheetPart.Worksheet.GetFirstChild<Columns>();
            columns = new Columns();

            columns.Append(new Column() { Min = 1, Max = 9, Width = 5, CustomWidth = true, Hidden = true });  // Type
            columns.Append(new Column() { Min = 2, Max = 9, Width = 60, CustomWidth = true });  // Title
            columns.Append(new Column() { Min = 3, Max = 9, Width = 40, CustomWidth = true }); // ConterPart
            columns.Append(new Column() { Min = 4, Max = 9, Width = 15, CustomWidth = true }); // Organiz
            columns.Append(new Column() { Min = 5, Max = 9, Width = 15, CustomWidth = true }); // Date
            columns.Append(new Column() { Min = 6, Max = 9, Width = 20, CustomWidth = true }); // Number
            columns.Append(new Column() { Min = 7, Max = 9, Width = 15, CustomWidth = true }); // Summ
            columns.Append(new Column() { Min = 8, Max = 9, Width = 10, CustomWidth = true }); // isUPD
            columns.Append(new Column() { Min = 9, Max = 9, Width = 40, CustomWidth = true }); // Comment

            worksheetPart.Worksheet.InsertAt(columns, 0);
            //return columns;
        }

        // Setting styles
        internal protected Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts =   new Fonts(
                new Font(new FontSize() { Val = 10 }),
                            new Font(new FontSize() { Val = 10 }, new Bold(), new Color() { Rgb = "FFFFFF" } ));

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                                new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                                new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } }) // Index 2 - Header
                                { PatternType = PatternValues.Solid }), // index 2 - Header
                                new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "fcf803" } }) // Index 3 - Body hilight
                                { PatternType = PatternValues.Solid }), // index 3 - Unmatched cell in document
                                new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "a9acb0" } }) // Index 3 - Body hilight
                                { PatternType = PatternValues.Solid }) // index 4 - Hilight no document in UPP 
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
                    new CellFormat { FontId = 0, FillId = 3, BorderId = 0, ApplyFill = true }, // Unmatched cell in document
                    new CellFormat { FontId = 0, FillId = 4, BorderId = 0, ApplyFill = true }  // Hilight no document in UPP 
                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }

    }
}
