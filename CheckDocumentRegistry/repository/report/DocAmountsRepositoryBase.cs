namespace RegComparator
{
    public abstract class DocAmountsRepositoryBase
    {
        public DocAmountsBase Amounts1CDO;
        public DocAmountsBase AmountsReg;

        public DocAmountsRepositoryBase(DocAmountsBase amounts1CDO, DocAmountsBase amountsReg)
        {
            Amounts1CDO = amounts1CDO;
            AmountsReg = amountsReg;
        }
    }
}
