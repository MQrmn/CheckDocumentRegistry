using System.Text.Json;

namespace RegComparator
{
    public class ReaderJSON<T>
    {
        internal protected T? GetJSON(string filePathParams)
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
