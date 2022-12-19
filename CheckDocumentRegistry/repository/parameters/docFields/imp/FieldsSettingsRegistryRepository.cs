namespace RegComparator
{
    public class FieldsSettingsRegistryRepository : FieldsSettingsRepositoryBase
    {
        public FieldsSettingsRegistryRepository(byte workMode = 1) 
        {
            CommonDocFieldsSettings = new DocFieldsCommon();
            if (workMode == 1)
                SpecDocFieldsSettings = new DocFields1CKA();
            else
                SpecDocFieldsSettings = new DocFields1CUPP();
            SetDefaults();
        }
    }
}
