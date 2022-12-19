namespace RegComparator
{
    public class DocRepositoryFiller
    {
        private DocLoader _loader;
        private FieldsSettingsRepositoryBase _fieldsSetRepo;
        private DocRepositoryBase _docRepo;
        private string[] _sourceDocSpreadsheetPath;
        private string[] _skipDocSpreadsheetPath;

        public DocRepositoryFiller(
                                    DocLoader loader,
                                    FieldsSettingsRepositoryBase fieldsSetRepo,
                                    DocRepositoryBase docRepo,
                                    string[] sourceDocSpreadsheetPath,
                                    string[] skipDocSpreadsheetPath
                                    )
        {
            _loader = loader;
            _fieldsSetRepo = fieldsSetRepo;
            _docRepo = docRepo;
            _sourceDocSpreadsheetPath = sourceDocSpreadsheetPath;
            _skipDocSpreadsheetPath = skipDocSpreadsheetPath;

        }

        public void FillRepository()
        {
            FillSourceDocs();
            FillSkipDocs();
        }

        private void FillSourceDocs()
        {
            _loader.GetDocObjectList(   _sourceDocSpreadsheetPath, 
                                        _docRepo.AddSourceDoc, 
                                        _fieldsSetRepo.SpecDocFieldsSettings
                                        );
        }

        private void FillSkipDocs()
        {
            _loader.GetDocObjectList(   _skipDocSpreadsheetPath, 
                                        _docRepo.AddSkippedDoc, 
                                        _fieldsSetRepo.CommonDocFieldsSettings
                                        );
        }

    }
}
