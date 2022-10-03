using System.Text;
using System.Text.Json;

namespace CheckDocumentRegistry
{
    internal class ParamsReadWriteJSON 
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
                arguments.SetDefaults();
                string jsonstring = JsonSerializer.Serialize(arguments);
                File.WriteAllText(filePath, jsonstring, Encoding.UTF8);
                Directory.CreateDirectory("input");
                Directory.CreateDirectory("output");

                Console.WriteLine("Файл конфигурации не считан.");
                Console.WriteLine("Файл конфигурации по умолчанию создан в папке приложения.");
                Console.WriteLine("Нажмите любую клавишу для завершения работы приложения.");
                Console.ReadKey();
                Environment.Exit(0);
            }
            return arguments;
        }
    }
}

