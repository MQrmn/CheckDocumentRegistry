namespace RegComparator
{
    public abstract class ProgramParamsRepositoryBase
    {
        private protected RootConfigFilePath _rootConfig;
        private protected IObjsConverter _objConverter;
        public CommonParams Common;
        public SpreadsheetsPaths1CDO Spreadsheets1CDO;
        public SpreadsheetsPathsRegistry SpreadsheetsRegistry;

        private protected abstract void GetObj<T>(ProgramParametersBase field, string path, Action verify, Action setDefaults) where T : ProgramParametersBase;

        private protected abstract void SetField<T>(ProgramParametersBase field);

        private protected abstract void PutObj(object obj, string path);
    }
}
