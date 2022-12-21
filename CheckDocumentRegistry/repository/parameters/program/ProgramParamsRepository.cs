namespace RegComparator
{
    public class ProgramParamsRepository : ProgramParamsRepositoryBase
    {
        public ProgramParamsRepository(IObjsSerialiser objSerialiser, RootConfigFilePath rootConfig)
        {
            _rootConfig = rootConfig;
            _objSerialiser = objSerialiser;
            Common = new CommonParams();
            Spreadsheets1CDO = new SpreadsheetsPaths1CDO();
            SpreadsheetsRegistry = new SpreadsheetsPathsRegistry();

            GetObj<CommonParams>
                (   Common, _rootConfig.CommonParamsFilePath, Common.VerifyFields, Common.SetDefaults);
            
            GetObj<SpreadsheetsPaths1CDO>
                (   Spreadsheets1CDO, Common.SpreadsheetParams1CDO, 
                    Spreadsheets1CDO.VerifyFields, Spreadsheets1CDO.SetDefaults);
            
            GetObj<SpreadsheetsPathsRegistry>
                (   SpreadsheetsRegistry, Common.SpreadsheetParamsRegistry, 
                    SpreadsheetsRegistry.VerifyFields, SpreadsheetsRegistry.SetDefaults);
        }

        private protected override void SetField<T>(IParameters field)
        {
            throw new NotImplementedException();
        }

        private protected override void GetObj<T>(T obj, string path, Action verify, Action setDefaults)
        {
            try
            {
                obj = _objSerialiser.GetObj<T>(path);

                verify();
            }
            catch
            {
                Console.WriteLine("Не удалось прочитать файл конфигурации " + path);
                setDefaults();
                Console.WriteLine("Установлена конфигурация по умолчанию ");
                PutObj(obj, path);
            }           

        }

        private protected override void PutObj(object obj, string path)
        {
            try
            {
                _objSerialiser.PutObj(obj, path);
                Console.WriteLine("Записан файл конфигурации по умолчанию " + path);
            }
            catch
            {
                Console.WriteLine("Не удалось записать файл конфигурации по умолчанию " + path);
            }
        }


    }
}
