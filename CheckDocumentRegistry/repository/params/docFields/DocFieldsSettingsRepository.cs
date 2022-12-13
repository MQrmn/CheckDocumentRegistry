namespace RegComparator
{
    internal class DocFieldsSettingsRepository
    {
        public DocFields1CDO DocFieldsDO;
        public DocFieldsCommon DocFieldsCmn;
        public DocFieldsReg1CKA DocFieldsKA;
        public DocFieldsReg1CUPP DocFieldsRegUPP;

        public DocFieldsSettingsRepository()
        {
            DocFieldsDO = new();
            DocFieldsCmn = new();
            DocFieldsKA = new();
            DocFieldsRegUPP = new();

            DocFieldsDO.SetDefaults();
            DocFieldsCmn.SetDefaults();
            DocFieldsKA.SetDefaults();
            DocFieldsRegUPP.SetDefaults();
        }

    }
}
