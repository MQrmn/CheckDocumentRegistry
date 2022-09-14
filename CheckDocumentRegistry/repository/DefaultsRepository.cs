using System.Text.Json;
using System.Text.Json.Serialization;

// https://docs.microsoft.com/ru-ru/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-6-0

namespace CheckDocumentRegistry
{
    internal class DefaultsRepository 
    {
        public static Arguments GetDefaults()
        {
            Arguments arguments = new();

            string filePath = "defaults.json";

            try
            {
                string jsonString = File.ReadAllText(filePath);
                arguments = JsonSerializer.Deserialize<Arguments>(jsonString)!;
            }
            catch
            {
                arguments = new Arguments(isDefault: true);
                string jsonstring = JsonSerializer.Serialize(arguments);
                File.WriteAllText("defaults.json", jsonstring);

                Console.WriteLine("Configuration file could not be read.");
                Console.WriteLine("The template of the configuration file was created in the folder with the application.");
                Console.WriteLine(@"Note: if you want to set absolute paths, you must use the format: C\:Folder\\\Folder\\...\\");
                Console.WriteLine("The application continues to work with the default settings.\n");
            }
            return arguments;
        }
    }
}

