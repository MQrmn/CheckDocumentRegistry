namespace RegComparator
{
    public class ProgramParamsRepository : ProgramParamsRepositoryBase
    {
        public ProgramParamsRepository(IObjsConverter objSerialiser, RootConfigFilePath rootConfig)
        {
            _rootConfig = rootConfig;
            _objConverter = objSerialiser;
            Common = new CommonParams();
            Spreadsheets1CDO = new SpreadsheetsPaths1CDO();
            SpreadsheetsRegistry = new SpreadsheetsPathsRegistry();


            //Console.WriteLine( Common.GetHashCode());
            GetObj<CommonParams>
                (   Common, _rootConfig.CommonParamsFilePath, Common.VerifyFields, Common.SetDefaults);

            Console.WriteLine(Spreadsheets1CDO.GetHashCode());
            GetObj<SpreadsheetsPaths1CDO>
                (   Spreadsheets1CDO, Common.SpreadsheetParams1CDO, 
                    Spreadsheets1CDO.VerifyFields, Spreadsheets1CDO.SetDefaults);

            Console.WriteLine(SpreadsheetsRegistry.GetHashCode());
            GetObj<SpreadsheetsPathsRegistry>
                (   SpreadsheetsRegistry, Common.SpreadsheetParamsRegistry, 
                    SpreadsheetsRegistry.VerifyFields, SpreadsheetsRegistry.SetDefaults);
        }

        private protected override void SetField<T>(ProgramParametersBase field)
        {
            throw new NotImplementedException();
        }

        private protected override void GetObj<T>(ProgramParametersBase obj, string path, Action verify, Action setDefaults) where T : class
        {
            try
            {
                Console.WriteLine("HashCode " + obj.GetHashCode());
                obj = _objConverter.GetObj<T>(path);
                Console.WriteLine("HashCode " + obj.GetHashCode());
                obj.VerifyFields();
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
