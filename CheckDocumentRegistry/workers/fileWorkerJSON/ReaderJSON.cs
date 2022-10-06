using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CheckDocumentRegistry
{
    internal class ReaderJSON<T>
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
                Console.WriteLine("Не удалось прочитать файл " + filePathParams );
                Console.ResetColor();

            }
            return deserialisedJsonData;
        }
    }
}
