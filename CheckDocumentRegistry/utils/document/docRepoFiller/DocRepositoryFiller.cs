namespace RegComparator
{
    public class DocRepositoryFiller
    {
        private DocLoader _loader;
        private FieldsSettingsRepositoryBase _fieldsSetRepo;
        private DocRepositoryBase _docRepo;
        private SpreadsheetsPathsBase _spreadsheetsPaths;

        public DocRepositoryFiller(
                                    DocLoader loader,
                                    FieldsSettingsRepositoryBase fieldsSetRepo,
                                    DocRepositoryBase docRepo,
                                    SpreadsheetsPathsBase spreadsheetsPaths
                                    )
        {
            _loader = loader;
            _fieldsSetRepo = fieldsSetRepo;
            _docRepo = docRepo;
            _spreadsheetsPaths = spreadsheetsPaths;
        }

        public void FillRepository()
        {
            FillSourceDocs();
            FillSkipDocs();
        }

        private void FillSourceDocs()
        {
            _loader.GetDocObjectList(   _spreadsheetsPaths.Source, 
                                        _docRepo.AddSourceDoc, 
                                        _fieldsSetRepo.SpecDocFieldsSettings
                                        );
        }

        private void FillSkipDocs()
        {
            _loader.GetDocObjectList(   _spreadsheetsPaths.Skipped, 
                                        _docRepo.AddSkippedDoc, 
                                        _fieldsSetRepo.CommonDocFieldsSettings
                                        );
        }

    }
}
