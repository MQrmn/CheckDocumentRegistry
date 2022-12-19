namespace RegComparator
{
    public abstract class FieldsSettingsRepositoryBase
    {
        public DocFieldsBase FieldsDO;
        public DocFieldsBase FieldsRegistry;
        public DocFieldsBase FieldsCmn;

        public DocFields1CDO DocFieldsDO;
        public DocFieldsCommon DocFieldsCmn;
        public DocFields1CKA DocFieldsKA;
        public DocFields1CUPP DocFieldsRegUPP;

        public DocFieldsBase SpecDocFieldsSettings;
        public DocFieldsBase CommonDocFieldsSettings;

        public void SetDefaults()
        {
            SpecDocFieldsSettings.SetDefaults();
            CommonDocFieldsSettings.SetDefaults();
        }

        private protected void SetDefaults_old()
        {
            FieldsDO.SetDefaults();
            FieldsRegistry.SetDefaults();
            FieldsCmn.SetDefaults();
    }
    }
}
