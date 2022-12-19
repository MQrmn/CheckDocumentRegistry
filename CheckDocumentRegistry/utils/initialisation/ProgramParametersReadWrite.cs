namespace RegComparator
{
    public class WorkParametersReadWrite
    {

        internal protected string parmetersFilePath;

        internal protected WorkParametersReadWrite(string filePath)
        {
            this.parmetersFilePath = filePath;
        }


        internal protected CommonParams GetProgramParameters()
        {
            CommonParams parameters = ReadProgramParameters();
            if (parameters is null)
            {
                parameters = new CommonParams();
                parameters.SetDefaults();
                this.WriteDefaultParameters(parameters);
            }
            return parameters;
        }


        internal protected CommonParams ReadProgramParameters()
        {
            CommonParams? programParameters;
            ReaderJSON<CommonParams> readerJSON = new();
            programParameters = readerJSON.GetJSON(this.parmetersFilePath);

            return programParameters;
        }

        internal protected void WriteDefaultParameters(CommonParams programParameters)
        {
            WriterJSON<CommonParams> writerJSON = new();
            writerJSON.WriteFileJSON(programParameters, parmetersFilePath);

            Console.WriteLine($"Файл конфигурации по умолчанию создан в папке приложения: {this.parmetersFilePath}");
            Console.WriteLine("Нажмите любую клавишу для завершения работы приложения.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
