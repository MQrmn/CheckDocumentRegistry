namespace RegComparator
{
    public abstract class ProgramParamsRepositoryBase
    {
        private protected RootConfig _rootConfig;
        private protected IObjsConverter _objConverter;
        private protected IFileExistChecker _fileExistChecker;
        public MainParams Common;
        public SpreadsheetsPaths1CDO Spreadsheets1CDO;
        public SpreadsheetsPathsRegistry SpreadsheetsRegistry;

        public abstract event EventHandler<string>? ErrNotify;
        public abstract event EventHandler<string>? Notify;

        public abstract void FillRepository();
        private protected abstract T GetObj<T>(string path) where T : ProgramParametersBase;
        private protected abstract void PutObj(object obj, string path);
    }
}
