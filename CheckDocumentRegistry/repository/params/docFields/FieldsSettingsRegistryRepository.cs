namespace RegComparator
{
    public class FieldsSettingsRegistryRepository : FieldsSettingsRepositoryBase
    {
        public FieldsSettingsRegistryRepository() 
        {
            CommonDocFieldsSettings = new DocFieldsCommon();
            SpecDocFieldsSettings = new DocFields1CDO();
            SetDefaults();
        }
    }
}
