
namespace CheckDocumentRegistry
{
    internal class InternalParameters
    {

        readonly internal string ProgramParametersFilePath;
        readonly internal string LastCompareDocumentsAnountsFilePath;

        internal InternalParameters()
        {

            this.ProgramParametersFilePath = "params.json";
            this.LastCompareDocumentsAnountsFilePath = "lastCompareAmounts.json";
        }

    }
}
