
namespace RegComparator
{
    public class ConfigFilesPath
    {

        internal protected string ProgramParametersFilePath;
        readonly internal protected string LastCompareDocumentsAnountsFilePath;
        internal protected ConfigFilesPath()
        {

            this.ProgramParametersFilePath = "params.json";
            this.LastCompareDocumentsAnountsFilePath = "lastCompareAmounts.json";
        }

    }
}
