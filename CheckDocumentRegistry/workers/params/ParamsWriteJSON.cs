using System.Text;
using System.Text.Json;

namespace CheckDocumentRegistry
{
    internal class ParamsWriteJSON
    {
        internal static void WriteDefaultParams(ChangeableParameters programParameters, string filePathParams)
        {
            string jsonstring = JsonSerializer.Serialize(programParameters);
            File.WriteAllText(filePathParams, jsonstring, Encoding.UTF8);

            Console.WriteLine($"Файл конфигурации по умолчанию создан в папке приложения: {filePathParams}");
            Console.WriteLine("Нажмите любую клавишу для завершения работы приложения.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
