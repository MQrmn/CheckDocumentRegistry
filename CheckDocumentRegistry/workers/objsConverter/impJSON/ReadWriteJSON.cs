using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace RegComparator
{
    public class ReadWriteJSON : IObjsConverter
    {
        public event EventHandler<string>? ErrNotify;
        public T? GetObj<T>(string path)
        {
            string jsonString;

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    jsonString = reader.ReadToEnd();
                }

                var options = new JsonSerializerOptions { IncludeFields = true };
                var obj = JsonSerializer.Deserialize<T>(jsonString, options)!;
                return obj;

            }
            catch
            {
                ErrNotify?.Invoke(this, "Не удалось прочитать файл: " + path);
                throw new Exception();
            }
        }

        public void PutObj(object obj, string path)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true,
                IncludeFields = true
            };

            string jsonstring = JsonSerializer.Serialize(obj, options);

            try
            {
                using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
                {
                    writer.WriteLineAsync(jsonstring);
                }
            }
            catch
            {
                ErrNotify?.Invoke(this, "Не удалось записать файл: " + path);
            }
        }


    }
}
