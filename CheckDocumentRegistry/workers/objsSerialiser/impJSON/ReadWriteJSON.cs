using System.Text.Json;

namespace RegComparator
{
    public class ReadWriteJSON : IObjsSerialiser
    {
        public T? GetObj<T>(string filePathParams)
        {
            T? deserialisedJsonData;

            try
            {
                string jsonString = File.ReadAllText(filePathParams);
                deserialisedJsonData = JsonSerializer.Deserialize<T>(jsonString);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Не удалось прочитать файл " + filePathParams );
                Console.ResetColor();
                throw new Exception();
            }
            return deserialisedJsonData;
        }
    }
}
