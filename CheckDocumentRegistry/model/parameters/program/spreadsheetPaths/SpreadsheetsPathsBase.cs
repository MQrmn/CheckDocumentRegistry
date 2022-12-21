namespace RegComparator
{
    public abstract class SpreadsheetsPathsBase : ProgramParametersBase
    {
        public string[] Source;
        public string[] Skipped;
        public string Matched;
        public string Unmatched;
        public string Excepted;
        //public abstract void SetDefaults();
        public override void VerifyFields()
        {
            if (Source?[0] == string.Empty || Source?[0] is null)
                throw new Exception();
            if (Unmatched == string.Empty || Unmatched is null)
                throw new Exception();
        }
    }
}
