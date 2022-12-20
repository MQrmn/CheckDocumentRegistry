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
                (   Common, _rootConfig.CommonParamsFilePath, Common.VerifyFields, Common.SetDefaults);
            
            GetObj<SpreadsheetsPaths1CDO>
                (   Spreadsheets1CDO, _rootConfig.CommonParamsFilePath, 
                    Spreadsheets1CDO.VerifyFields, Spreadsheets1CDO.SetDefaults);
            
            GetObj<SpreadsheetsPathsRegistry>
                (   SpreadsheetsRegistry, _rootConfig.CommonParamsFilePath, 
                    SpreadsheetsRegistry.VerifyFields, SpreadsheetsRegistry.SetDefaults);
        }

        private protected override void SetField<T>(IParameters field)
        {
            throw new NotImplementedException();
        }

        private protected override void GetObj<T>(T field, string path, Action verify, Action setDefaults)
        {
            try
            {
                field = _readWriteJSON.GetObj<T>(path);
                verify();
            }
            catch
            {
                setDefaults();
            }           

        }


    }
}
