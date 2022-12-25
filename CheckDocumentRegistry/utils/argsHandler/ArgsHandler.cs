namespace RegComparator
{
    internal class ArgsHandler : IArgsHandler
    {
        private string[] _args;
        
        public ArgsHandler(string[] args)
        {
            _args = args;
        }

        public RootConfig GetParams()
        {
            RootConfig rootConfig = new(GetMainParamsPath(_args));
            rootConfig.VerifyFields();
            return rootConfig;
        }

        private string? GetMainParamsPath(string[] args)
        {
            for (var i = 0; i < args.Length; i++) 
            {
                if (args[i] == "-c" )
                {
                    return args[i+1];
                }
            }
            return null;
        }
    }
}
