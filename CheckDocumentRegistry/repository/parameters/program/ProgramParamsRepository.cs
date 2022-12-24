namespace RegComparator
{
    public class ProgramParamsRepository : ProgramParamsRepositoryBase
    {
        public ProgramParamsRepository(RootConfig rootConfig, IObjsConverter objSerialiser, IFileExistChecker fileExistChecker)
        {
            _rootConfig = rootConfig;
            _objConverter = objSerialiser;
            _fileExistChecker = fileExistChecker;

            Common = GetObj<MainParams>(_rootConfig.MainParamsFilePath);
            Spreadsheets1CDO = GetObj<SpreadsheetsPaths1CDO>(Common.SpreadsheetParams1CDO);
            SpreadsheetsRegistry = GetObj<SpreadsheetsPathsRegistry>( Common.SpreadsheetParamsRegistry);
            
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
                Console.WriteLine("Не удалось прочитать файл конфигурации " + path);
                var obj = (T)Activator.CreateInstance(typeof(T));
                obj.SetDefaults();
                Console.WriteLine("Установлена конфигурация по умолчанию ");
                PutObj(obj, path);
                return obj;
            }
        }

        private protected override void PutObj(object obj, string path)
        {
            try
            {
                _objConverter.PutObj(obj, path);
                Console.WriteLine("Записан файл конфигурации по умолчанию " + path);
            }
            catch
            {
                Console.WriteLine("Не удалось записать файл конфигурации по умолчанию " + path);
            }
        }
    }
}
