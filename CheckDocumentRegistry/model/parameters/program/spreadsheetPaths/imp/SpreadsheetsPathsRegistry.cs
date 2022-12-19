namespace RegComparator
{
    public class SpreadsheetsPathsRegistry : SpreadsheetsPathsBase
    {
        public override void SetDefaults()
        {
            Source = new string[] { "C:\\1C\\АРХИВНЫЕ ДОКУМЕНТЫ\\ВЫГРУЗКА ИЗ РЕЕСТРА\\KASF.xlsx", 
                                    "C:\\1C\\АРХИВНЫЕ ДОКУМЕНТЫ\\ВЫГРУЗКА ИЗ РЕЕСТРА\\KATN.xlsx" };
            Skipped = new string[] { "C:\\1C\\АРХИВНЫЕ ДОКУМЕНТЫ\\ИСКЛЮЧЕНИЯ ИЗ ПРОВЕРКИ\\PassDocRegistry.xlsx" };
            Matched = "C:\\1C\\АРХИВНЫЕ ДОКУМЕНТЫ\\РЕЗУЛЬТАТЫ СРАВНЕНИЯ\\MatchedDocRegistry.xlsx";
            Unmatched = "C:\\1C\\АРХИВНЫЕ ДОКУМЕНТЫ\\РЕЗУЛЬТАТЫ СРАВНЕНИЯ\\UnmatchedDocRegistry.xlsx";
            Excepted = "C:\\1C\\АРХИВНЫЕ ДОКУМЕНТЫ\\РЕЗУЛЬТАТЫ СРАВНЕНИЯ\\exceptedDocRegistry.xlsx";
        }
    }
}
