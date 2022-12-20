
namespace RegComparator
{
    public class RootConfigFilePath : IParameters
    {
        public string CommonParamsFilePath;
        public RootConfigFilePath()
        {
            SetDefaults();
        }

        public void SetDefaults()
        {
            CommonParamsFilePath = "CommonParams.json";
        }

        public void VerifyFields()
        {
            if (CommonParamsFilePath == string.Empty || CommonParamsFilePath is null)
                throw new Exception();
        }
    }
}
