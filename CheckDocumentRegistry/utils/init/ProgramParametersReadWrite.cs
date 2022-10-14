namespace RegComparator
{
    public class ProgramParametersReadWrite
    {

        internal protected string parmetersFilePath;

        internal protected ProgramParametersReadWrite(string filePath)
        {
            this.parmetersFilePath = filePath;
        }


        internal protected UserParameters GetProgramParameters()
        {
            UserParameters parameters = ReadProgramParameters();
            if (parameters is null)
            {
                parameters = new UserParameters();
                parameters.SetDefaults();
                this.WriteDefaultParameters(parameters);
            }
            return parameters;
        }


        internal protected UserParameters ReadProgramParameters()
        {
            UserParameters? programParameters;
            ReaderJSON<UserParameters> readerJSON = new();
            programParameters = readerJSON.GetJSON(this.parmetersFilePath);

            return programParameters;
        }

        internal protected void WriteDefaultParameters(UserParameters programParameters)
        {
            WriterJSON<UserParameters> writerJSON = new();
            writerJSON.WriteFileJSON(programParameters, parmetersFilePath);

            Console.WriteLine($"Файл конфигурации по умолчанию создан в папке приложения: {this.parmetersFilePath}");
            Console.WriteLine("Нажмите любую клавишу для завершения работы приложения.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
