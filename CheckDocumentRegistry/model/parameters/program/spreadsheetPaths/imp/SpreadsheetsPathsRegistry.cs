namespace RegComparator
{
    public class SpreadsheetsPathsRegistry : SpreadsheetsPathsBase
    {
        public override void SetDefaults()
        {
            Source = new string[] { "KASF.xlsx", 
                                    "KATN.xlsx" };
            Skipped = new string[] { "SkipReg.xlsx" };
            Matched = "MatchedReg.xlsx";
            Unmatched = "UnmatchedReg.xlsx";
            Excepted = "ExceptedReg.xlsx";
        }
    }
}
