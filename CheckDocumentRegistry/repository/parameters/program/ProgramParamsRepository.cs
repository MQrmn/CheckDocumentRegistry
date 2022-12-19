namespace RegComparator
{
    public class ProgramParamsRepository : ProgramParamsRepositoryBase
    {
        public ProgramParamsRepository(IObjsSerialiser readWriteJSON, RootConfigFilePath rootConfig)
        {
            _rootConfig = rootConfig;
            _readWriteJSON = readWriteJSON;
            Common = new CommonParams();
            Spreadsheets1CDO = new SpreadsheetsPaths1CDO();
            SpreadsheetsRegistry = new SpreadsheetsPathsRegistry();

            GetObj<CommonParams>
                (Common, _rootConfig.CommonParamsFilePath, Common.SetDefaults);
            
            GetObj<SpreadsheetsPaths1CDO>
                (Spreadsheets1CDO, _rootConfig.CommonParamsFilePath, Spreadsheets1CDO.SetDefaults);
            
            GetObj<SpreadsheetsPathsRegistry>
                (SpreadsheetsRegistry, _rootConfig.CommonParamsFilePath, SpreadsheetsRegistry.SetDefaults);
        }


        private protected override void GetObj<T>(T field, string path, Action setDefaults)
        {
            try
            {
                field = _readWriteJSON.GetObj<T>(path);
            }
            catch
            {
                setDefaults();
            }           

        }
    }
}
