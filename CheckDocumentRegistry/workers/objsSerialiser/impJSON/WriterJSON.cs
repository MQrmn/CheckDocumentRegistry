using System.Text;
using System.Text.Json;

namespace RegComparator
{
    public class WriterJSON
    {
        internal protected void Write<T>(T obj, string filePath)
        {
            string jsonstring = JsonSerializer.Serialize(obj);
            try
            {
                File.WriteAllText(filePath, jsonstring, Encoding.UTF8);
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