namespace RegComparator
{
    public abstract class DocAmountsRepositoryBase
    {
        public DocAmountBase Amounts1CDO;
        public DocAmountBase AmountsReg;

        public DocAmountsRepositoryBase(DocAmountBase amounts1CDO, DocAmountBase amountsReg)
        {
            Amounts1CDO = amounts1CDO;
            AmountsReg = amountsReg;
        }
    }
}
