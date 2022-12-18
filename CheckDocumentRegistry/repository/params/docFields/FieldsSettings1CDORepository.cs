namespace RegComparator
{
    public class FieldsSettings1CDORepository : FieldsSettingsRepositoryBase
    {
        public FieldsSettings1CDORepository(byte workMode = 1)
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
