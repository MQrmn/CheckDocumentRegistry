using System.Text;

namespace RegComparator
{
    internal class AdditionalInfoWriteTXT
    {
        internal static void WriteAdditionalInfo()
        {
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


            File.WriteAllText(filePathAboutProgram, aboutProgram, Encoding.UTF8);
            File.WriteAllText(filePathAboutParams, aboutConfig, Encoding.UTF8);
            
            Console.WriteLine($"Пояснения к файлу конфигурации в папке приложения: {filePathAboutParams}");
        }

        

    }

}
