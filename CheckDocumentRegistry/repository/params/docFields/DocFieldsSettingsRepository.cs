namespace RegComparator
{
    internal class DocFieldsSettingsRepository : DocFieldsSettingsRepositoryBase
    {
        public DocFieldsSettingsRepository(byte workMode = 1)
        {
            FieldsDO = new DocFields1CDO();
            FieldsCmn = new DocFieldsCommon();

            if (workMode == 1)
                FieldsRegistry = new DocFieldsReg1CKA();
            else
                FieldsRegistry = new DocFieldsReg1CUPP();

            SetDefaults();
        }

    }
}
