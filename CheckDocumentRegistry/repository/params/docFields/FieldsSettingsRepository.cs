namespace RegComparator
{
    internal class FieldsSettingsRepository : FieldsSettingsRepositoryBase
    {
        public FieldsSettingsRepository(byte workMode = 1)
        {
            FieldsDO = new DocFields1CDO();
            FieldsCmn = new DocFieldsCommon();

            if (workMode == 1)
                FieldsRegistry = new DocFields1CKA();
            else
                FieldsRegistry = new DocFields1CUPP();

            SetDefaults_old();
        }

    }
}
