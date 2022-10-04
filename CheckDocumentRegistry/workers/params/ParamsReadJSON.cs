using System.Text;
using System.Text.Json;

namespace CheckDocumentRegistry
{
    public class ParamsReadJSON 
    {
        public static ChangeableParameters GetProgramParameters(string filePathParams)
        {
            ChangeableParameters? programParameters;

            try
            {
                string jsonString = File.ReadAllText(filePathParams);
                programParameters = JsonSerializer.Deserialize<ChangeableParameters>(jsonString);
            }
            catch
            {
                programParameters = null;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Файл конфигурации не считан.");
                Console.ResetColor();
            }
            return programParameters;
        }
    }
}

