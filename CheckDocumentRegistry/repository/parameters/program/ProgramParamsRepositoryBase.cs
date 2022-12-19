namespace RegComparator
{
    public abstract class ProgramParamsRepositoryBase
    {
        private protected RootConfigFilePath _rootConfig;
        private protected IObjsSerialiser _readWriteJSON;
        public CommonParams Common;
        public SpreadsheetsPaths1CDO Spreadsheets1CDO;
        public SpreadsheetsPathsRegistry SpreadsheetsRegistry;

        private protected abstract void GetObj<T>(T field, string path, Action setDefaults);
    }
}
