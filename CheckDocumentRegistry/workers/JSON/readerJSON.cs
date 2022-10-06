using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CheckDocumentRegistry
{
    internal class readerJSON<T>
    {
        public T? GetJSON(string filePathParams)
        {
            T? deserialisedJsonData;

            try
            {
                string jsonString = File.ReadAllText(filePathParams);
                deserialisedJsonData = JsonSerializer.Deserialize<T>(jsonString);
            }
            catch
            {
                deserialisedJsonData = default(T);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Файл " + filePathParams + " не удалось считать.");
                Console.ResetColor();

            }
            return deserialisedJsonData;
        }
    }
}
