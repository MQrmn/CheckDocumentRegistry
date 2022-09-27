using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDocumentRegistry
{
    internal class WorkAbilityChecker
    {
        public static void CheckSourceFiles(ProgramParameters programParameters)
        {
            bool doExist = File.Exists(programParameters.doSpreadSheetPath);
            bool uppExist = File.Exists(programParameters.uppSpreadSheetPath);

            if (doExist && uppExist)
            {
                return;
            }
            else
            {
                if (!doExist)
                {
                    Console.WriteLine($"Error: File \"{programParameters.doSpreadSheetPath}\" not found.");
                }
                else if (!uppExist)
                {
                    Console.WriteLine($"Error: File \"{programParameters.uppSpreadSheetPath}\" not found.");
                }
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
    }
}
