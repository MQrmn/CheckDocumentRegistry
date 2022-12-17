namespace RegComparator
{
    public abstract class DocFieldsSettingsRepositoryBase
    {
        public DocFieldsBase FieldsDO;
        public DocFieldsBase FieldsRegistry;
        public DocFieldsBase FieldsCmn;

        public DocFields1CDO DocFieldsDO;
        public DocFieldsCommon DocFieldsCmn;
        public DocFieldsReg1CKA DocFieldsKA;
        public DocFieldsReg1CUPP DocFieldsRegUPP;

        private protected void SetDefaults()
        {
            FieldsDO.SetDefaults();
            FieldsRegistry.SetDefaults();
            FieldsCmn.SetDefaults();
    }
    }
}
