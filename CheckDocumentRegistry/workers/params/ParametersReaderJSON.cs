using System.Text;
using System.Text.Json;

namespace CheckDocumentRegistry
{
    internal class ParametersReaderJSON 
    {
        public static ProgramParameters GetParameters()
        {
            ProgramParameters arguments;

            string filePath = "params.json";

            try
            {
                string jsonString = File.ReadAllText(filePath);
                arguments = JsonSerializer.Deserialize<ProgramParameters>(jsonString);
            }
            catch
            {
                arguments = new ProgramParameters();
                //arguments.SetDefaults();
                string jsonstring = JsonSerializer.Serialize(arguments);
                File.WriteAllText(filePath, jsonstring, Encoding.UTF8);
                Directory.CreateDirectory("input");
                Directory.CreateDirectory("output");

                Console.WriteLine("Configuration file could not be read.");
                Console.WriteLine("The template of the configuration file was created in the folder with the application.");
                Console.WriteLine("The application continues to work with the default settings.\n");
            }
            return arguments;
        }
    }
}

