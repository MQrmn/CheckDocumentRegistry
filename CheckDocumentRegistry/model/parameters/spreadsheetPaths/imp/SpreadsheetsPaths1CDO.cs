namespace RegComparator
{
    public class SpreadsheetsPaths1CDO : SpreadsheetsPathsBase
    {
        public override void SetDefaults()
        {
            Source = new string[] { "C:\\1C\\АРХИВНЫЕ ДОКУМЕНТЫ\\ВЫГРУЗКА ИЗ ДОКУМЕНТООБОРОТА\\DO.xlsx" };
            Skipped = new string[] { "C:\\1C\\АРХИВНЫЕ ДОКУМЕНТЫ\\ИСКЛЮЧЕНИЯ ИЗ ПРОВЕРКИ\\IgnoreDO.xlsx" };
            Matched = "C:\\1C\\АРХИВНЫЕ ДОКУМЕНТЫ\\РЕЗУЛЬТАТЫ СРАВНЕНИЯ\\MatchedDocRegistry.xlsx";
            Unmatched = "C:\\1C\\АРХИВНЫЕ ДОКУМЕНТЫ\\РЕЗУЛЬТАТЫ СРАВНЕНИЯ\\UnmatchedDocManagement.xlsx";
            Excepted = "C:\\1C\\АРХИВНЫЕ ДОКУМЕНТЫ\\РЕЗУЛЬТАТЫ СРАВНЕНИЯ\\exceptedDocManagement.xlsx";
        }
    }
}
