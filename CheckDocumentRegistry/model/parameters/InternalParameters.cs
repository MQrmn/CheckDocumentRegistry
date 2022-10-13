
namespace RegComparator
{
    public class InternalParameters
    {

        readonly public string ProgramParametersFilePath;
        readonly public string LastCompareDocumentsAnountsFilePath;
        public InternalParameters()
        {

            this.ProgramParametersFilePath = "params.json";
            this.LastCompareDocumentsAnountsFilePath = "lastCompareAmounts.json";
        }

    }
}
