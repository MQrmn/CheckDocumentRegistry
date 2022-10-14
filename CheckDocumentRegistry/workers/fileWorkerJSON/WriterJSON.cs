using System.Text;
using System.Text.Json;

namespace RegComparator
{
    public class WriterJSON<T>
    {
        internal protected bool WriteFileJSON(T dataForWrite, string filePath)
        {
            string jsonstring = JsonSerializer.Serialize(dataForWrite);
            try
            {
                File.WriteAllText(filePath, jsonstring, Encoding.UTF8);
                return true;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Не удалось записать файл " + filePath);
                Console.ResetColor();
                return false;
            }
        }
    }
}