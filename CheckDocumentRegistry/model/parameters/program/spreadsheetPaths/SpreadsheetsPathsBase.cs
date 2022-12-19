namespace RegComparator
{
    public abstract class SpreadsheetsPathsBase
    {
        public string[] Source;
        public string[] Skipped;
        public string Matched;
        public string Unmatched;
        public string Excepted;
        public abstract void SetDefaults();

    }

    
}
