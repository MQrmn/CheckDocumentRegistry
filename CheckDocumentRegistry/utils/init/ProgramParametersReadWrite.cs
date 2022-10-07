
namespace CheckDocumentRegistry
{
    internal class ProgramParametersReadWrite
    {

        internal string parmetersFilePath;

        internal ProgramParametersReadWrite(string filePath)
        {
            this.parmetersFilePath = filePath;
        }


        internal UserParameters GetProgramParameters()
        {
            UserParameters parameters = ReadProgramParameters();
            if (parameters is null)
            {
                parameters.SetDefaults();
                this.WriteDefaultParameters(parameters);
            }
            return parameters;
        }


        private UserParameters ReadProgramParameters()
        {
            UserParameters? programParameters;
            ReaderJSON<UserParameters> readerJSON = new();
            programParameters = readerJSON.GetJSON(this.parmetersFilePath);

            return programParameters;
        }

        private void WriteDefaultParameters(UserParameters programParameters)
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
