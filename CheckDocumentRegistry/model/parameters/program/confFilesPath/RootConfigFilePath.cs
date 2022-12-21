
namespace RegComparator
{
    public class RootConfigFilePath : ProgramParametersBase
    {
        public string CommonParamsFilePath;
        public RootConfigFilePath()
        {
            SetDefaults();
        }

        public override void SetDefaults()
        {
            CommonParamsFilePath = "CommonParams.json";
        }

        public override void VerifyFields()
        {
            if (CommonParamsFilePath == string.Empty || CommonParamsFilePath is null)
                throw new Exception();
        }
    }
}
