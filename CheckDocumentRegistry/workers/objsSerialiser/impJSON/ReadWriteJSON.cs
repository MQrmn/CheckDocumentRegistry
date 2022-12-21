using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public void PutObj(object obj, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
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
