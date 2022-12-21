namespace RegComparator
{
    public abstract class ProgramParamsRepositoryBase
    {
        private protected RootConfigFilePath _rootConfig;
        private protected IObjsConverter _objConverter;
        public MainParams Common;
        public SpreadsheetsPaths1CDO Spreadsheets1CDO;
        public SpreadsheetsPathsRegistry SpreadsheetsRegistry;

        private protected abstract T GetObj<T>(string path) where T : ProgramParametersBase;
        private protected abstract void PutObj(object obj, string path);
        public abstract List<string> GetSourcePaths();
        public abstract List<string> GetSkippedPaths();
        private protected abstract List<string> CreatePathList(string[][] paramsObjs);
        
    }
}
