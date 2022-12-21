namespace RegComparator
{
    public class ProgramParamsRepository : ProgramParamsRepositoryBase
    {
        public ProgramParamsRepository(IObjsConverter objSerialiser, RootConfigFilePath rootConfig)
        {
            _rootConfig = rootConfig;
            _objConverter = objSerialiser;

            Common = GetObj<CommonParams>(_rootConfig.CommonParamsFilePath);
            Spreadsheets1CDO = GetObj<SpreadsheetsPaths1CDO>(Common.SpreadsheetParams1CDO);
            SpreadsheetsRegistry = GetObj<SpreadsheetsPathsRegistry>( Common.SpreadsheetParamsRegistry);
        }

        private protected override void SetField<T>(ProgramParametersBase field)
        {
            throw new NotImplementedException();
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
