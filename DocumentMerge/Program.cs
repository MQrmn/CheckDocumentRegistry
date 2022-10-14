
namespace DocumentMerge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InternalParameters internalParameters;                      // Static parameters
            UserParameters userParameters;                              // Loaded from config file parameters

            void GetProgramParameters()
            {
                internalParameters = new();
                ProgramParametersReadWrite programParameters1 = new(internalParameters.ProgramParametersFilePath);
                userParameters = programParameters1.GetProgramParameters();
            }
        }
    }
}