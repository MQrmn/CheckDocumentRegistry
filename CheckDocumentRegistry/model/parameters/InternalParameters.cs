
namespace RegComparator
{
    public class InternalParameters
    {

        internal protected string ProgramParametersFilePath;
        readonly internal protected string LastCompareDocumentsAnountsFilePath;
        internal protected InternalParameters()
        {

            this.ProgramParametersFilePath = "params.json";
            this.LastCompareDocumentsAnountsFilePath = "lastCompareAmounts.json";
        }

    }
}
