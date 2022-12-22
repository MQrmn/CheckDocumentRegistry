namespace RegComparator
{
    public class RootConfig : ProgramParametersBase
    {
        public string? MainParamsFilePath;
        public RootConfig() {}
        public RootConfig(string? mainParamsFilePath)
        {
            MainParamsFilePath = mainParamsFilePath;
        }
        public override void VerifyFields()
        {
            if (MainParamsFilePath == string.Empty || MainParamsFilePath is null)
                SetDefaults();
        }
        public override void SetDefaults()
        {
            MainParamsFilePath = "MainParams.json";
        }
    }
}
