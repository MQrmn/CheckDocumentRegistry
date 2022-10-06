
namespace CheckDocumentRegistry
{
    internal class ProgramParametersReadWrite
    {

        internal string parmetersFilePath;

        internal ProgramParametersReadWrite(string filePath)
        {
            this.parmetersFilePath = filePath;
        }


        internal ChangeableParameters GetProgramParameters()
        {
            ChangeableParameters parameters = ReadProgramParameters();
            if (parameters is null)
            {
                parameters.SetDefaults();
                this.WriteDefaultParameters(parameters);
            }
            return parameters;
        }


        private ChangeableParameters ReadProgramParameters()
        {
            ChangeableParameters? programParameters;
            ReaderJSON<ChangeableParameters> readerJSON = new();
            programParameters = readerJSON.GetJSON(this.parmetersFilePath);

            return programParameters;
        }

        private void WriteDefaultParameters(ChangeableParameters programParameters)
        {
            WriterJSON<ChangeableParameters> writerJSON = new();
            writerJSON.WriteFileJSON(programParameters, parmetersFilePath);

            Console.WriteLine($"Файл конфигурации по умолчанию создан в папке приложения: {this.parmetersFilePath}");
            Console.WriteLine("Нажмите любую клавишу для завершения работы приложения.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
