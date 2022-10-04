using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CheckDocumentRegistry
{
    internal class WorkAbilityChecker
    {
        public static void CheckFiles(ChangeableParameters programParameters)
        {
            bool doExist = File.Exists(programParameters.doSpreadSheetPath);
            bool uppExist = File.Exists(programParameters.uppSpreadSheetPath);

            if (doExist && uppExist)
            {
                TryOpenSpreadSheet(programParameters.doSpreadSheetPath);
                TryOpenSpreadSheet(programParameters.uppSpreadSheetPath);
            }
            else
            {
                if (!doExist)
                {
                    WriteMessageExit($"Error: Файл \"{programParameters.doSpreadSheetPath}\" не найден.");
                }
                if (!uppExist)
                {
                    WriteMessageExit($"Error: Файл \"{programParameters.uppSpreadSheetPath}\" не найден.");
                }
            }

            TryCreateSpreadSheet(programParameters.passedDoPath);
            TryCreateSpreadSheet(programParameters.passedUppPath);
        }
        
        private static void TryOpenSpreadSheet(string filePath)
        {
            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
            }
            catch
            {
                WriteMessageExit($"Error: Невозможно открыть файл \"{filePath}\".");
            }
        }

        private static void TryCreateSpreadSheet(string filePath)
        {
            try
            {
                SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument
                .Create(filePath, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);

                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();
                workbookpart.Workbook.Save();
                spreadsheetDocument.Close();
            }
            catch
            {
                WriteMessageExit($"Error: Невозможно создать файл \"{filePath}\".");
            }
        }

        private static void WriteMessageExit(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
