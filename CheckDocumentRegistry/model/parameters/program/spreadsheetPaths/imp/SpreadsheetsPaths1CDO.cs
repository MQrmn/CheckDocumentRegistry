namespace RegComparator
{
    public class SpreadsheetsPaths1CDO : SpreadsheetsPathsBase
    {
        public override void SetDefaults()
        {
            Source = new string[] { "Src1CDO.xlsx" };
            Skipped = new string[] { "Skip1CDO.xlsx" };
            Matched = "Matched1CDO.xlsx";
            Unmatched = "Unmatched1CDO.xlsx";
            Excepted = "Excepted1CDO.xlsx";
        }
    }
}
