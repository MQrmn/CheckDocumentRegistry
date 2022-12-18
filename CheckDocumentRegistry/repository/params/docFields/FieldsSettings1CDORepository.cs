namespace RegComparator
{
    public class FieldsSettings1CDORepository : FieldsSettingsRepositoryBase
    {
        public FieldsSettings1CDORepository()
        {
            CommonDocFieldsSettings = new DocFieldsCommon();
            SpecDocFieldsSettings = new DocFields1CDO();
            SetDefaults();

            
        }
    }
}
