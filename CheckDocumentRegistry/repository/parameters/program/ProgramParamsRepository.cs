namespace RegComparator
{
    public class ProgramParamsRepository : ProgramParamsRepositoryBase
    {
        public override event EventHandler<string>? ErrNotify;
        public override event EventHandler<string>? Notify;
        public ProgramParamsRepository(RootConfig rootConfig, IObjsConverter objSerialiser, IFileExistChecker fileExistChecker)
        {
            _rootConfig = rootConfig;
            _objConverter = objSerialiser;
            _fileExistChecker = fileExistChecker;
        }

        public override void FillRepository() 
        {
            Common = GetObj<MainParams>(_rootConfig.MainParamsFilePath);
            Spreadsheets1CDO = GetObj<SpreadsheetsPaths1CDO>(Common.SpreadsheetParams1CDO);
            SpreadsheetsRegistry = GetObj<SpreadsheetsPathsRegistry>(Common.SpreadsheetParamsRegistry);

            // Check file existing
            _fileExistChecker.CheckCritical(Spreadsheets1CDO.Source);
            _fileExistChecker.CheckCritical(SpreadsheetsRegistry.Source);
            _fileExistChecker.CheckNonCritical(Spreadsheets1CDO.Skipped);
            _fileExistChecker.CheckNonCritical(SpreadsheetsRegistry.Skipped);
        }

        private protected override T GetObj<T>(string path)
        {
            try
            {
                var obj = _objConverter.GetObj<T>(path);
                obj.VerifyFields();
                return obj;
            }
            catch
            {
                ErrNotify?.Invoke(this, "Не удалось получить конфигурацию из файла: " + path);
                var obj = (T)Activator.CreateInstance(typeof(T));
                obj.SetDefaults();
                Notify?.Invoke(this, "Установлена конфигурация по умолчанию ");
                PutObj(obj, path);
                return obj;
            }
        }

        private protected override void PutObj(object obj, string path)
        {
            try
            {
                _objConverter.PutObj(obj, path);
                Notify?.Invoke(this, "Записан файл конфигурации по умолчанию: " + path);
            }
            catch
            {
                ErrNotify?.Invoke(this, "Не удалось записать файл конфигурации по умолчанию: " + path);
            }
        }
    }
}
