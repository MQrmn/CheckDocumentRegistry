
namespace RegComparator
{
    public class ConfFilesPath
    {

        internal protected string ProgramParametersFilePath;
        readonly internal protected string LastCompareDocumentsAnountsFilePath;
        internal protected ConfFilesPath()
        {

            this.ProgramParametersFilePath = "params.json";
            this.LastCompareDocumentsAnountsFilePath = "lastCompareAmounts.json";
        }

    }
}
