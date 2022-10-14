using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace RegComparator
{
    
    public class WorkAbilityChecker
    {
        
        internal protected static void CheckFiles(UserParameters programParameters)
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

        internal protected static void TryOpenSpreadSheet(string filePath)
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

        internal protected static void TryCreateSpreadSheet(string filePath)
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

        internal protected static void WriteMessageExit(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("Для завершения программы нажмите любую клавишу.");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
