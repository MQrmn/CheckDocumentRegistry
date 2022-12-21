using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegComparator
{
    public class ReadWriteJSON : IObjsConverter
    {
        public T? GetObj<T>(string filePathParams)
        {
            T? deserialisedJsonData;

            try
            {
                string jsonString = File.ReadAllText(filePathParams);
                var options = new JsonSerializerOptions { IncludeFields = true };
                deserialisedJsonData = JsonSerializer.Deserialize<T>(jsonString, options)!;

            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Не удалось прочитать файл " + filePathParams);
                Console.ResetColor();
                throw new Exception();
            }
            return deserialisedJsonData;
        }

        public void PutObj(object obj, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
            string jsonstring = JsonSerializer.Serialize(obj, options);

            try
            {
                File.WriteAllText(filePath, jsonstring);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Не удалось записать файл " + filePath);
                Console.ResetColor();
            }
        }


    }
}
