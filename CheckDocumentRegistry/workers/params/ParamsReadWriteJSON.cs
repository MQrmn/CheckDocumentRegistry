using System.Text;
using System.Text.Json;

namespace CheckDocumentRegistry
{
    internal class ParamsReadWriteJSON 
    {
        public static ProgramParameters GetParameters()
        {
            ProgramParameters arguments;

            string filePathParams = "params.json";
            string filePathAboutParams = "aboutParams.txt";
            string filePathAboutProgram = "aboutProgram.txt";

            string aboutProgram = "Ссылка на репозиторий: https://github.com/MQrmn/CheckDocumentRegistry";

            string aboutConfig = 
                "Пояснене к файлу конфигурации: \n" +
                "doSpreadSheetPath - Файл списка документов 1С:ДО\n" +
                "uppSpreadSheetPath - Файл реестра 1С:УПП\n" +
                "doIgnoreSpreadSheetPath - Файл игнорируемых документов 1С:ДО\n" +
                "uppIgnoreSpreadSheetPath - Файл игнорируемых документов 1С:УПП\n" +
                "matchedDoPath - Список совпавших документов для 1С:ДО\n" +
                "matchedUppPath - Список совпавших файлов для 1С:УПП\n" +
                "passedDoPath - Список не совпавших файлов для 1С:ДО\n" +
                "passedUppPath - Список не совпавших файлов для 1C:УПП\n" +
                "exceptedDoPath - Список записей в doSpreadSheetPath, в которых возникла ошибка чтения\n" +
                "exceptedUppPath - Список записей в uppSpreadSheetPath, в которых возникла ошибка чтения\n" +
                "printMatchedDocuments - Если \"true\", то создает документы  matchedDoPath и matchedUppPath\n" +
                "askAboutCloseProgram - Если \"true\", то перед окончанием работы просит нажать любую клавишу";

            try
            {
                string jsonString = File.ReadAllText(filePathParams);
                arguments = JsonSerializer.Deserialize<ProgramParameters>(jsonString);
            }
            catch
            {
                arguments = new ProgramParameters();
                arguments.SetDefaults();
                string jsonstring = JsonSerializer.Serialize(arguments);
                File.WriteAllText(filePathParams, jsonstring, Encoding.UTF8);
                File.WriteAllText(filePathAboutParams, aboutConfig, Encoding.UTF8);
                File.WriteAllText(filePathAboutProgram, aboutProgram, Encoding.UTF8);

                Directory.CreateDirectory("input");
                Directory.CreateDirectory("output");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Файл конфигурации не считан.");
                Console.ResetColor();

                Console.WriteLine($"Файл конфигурации по умолчанию создан в папке приложения: {filePathParams}");
                Console.WriteLine($"Пояснения к файлу конфигурации в папке приложения: {filePathAboutParams}");
                Console.WriteLine("Нажмите любую клавишу для завершения работы приложения.");
                Console.ReadKey();
                Environment.Exit(0);
            }
            return arguments;
        }
    }
}

