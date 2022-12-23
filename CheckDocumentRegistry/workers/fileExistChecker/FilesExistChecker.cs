namespace RegComparator
{
    public class FileExistChecker
    {
        public void CheckCritical(List<string> paths)
        {
            if (!Check(paths, true))
            {
                Console.WriteLine("Завершение работы программы");
                Environment.Exit(0);
            }
        }

        public void CheckNonCritical(List<string> paths)
        {
            Check(paths, false);
        }

        private bool Check(List<string> paths, bool isStrict)
        {
            //foreach (string? p in paths[0]) 
            for (var i = 0; i < paths.Count; i++)
            {
                bool isExist = File.Exists(paths[i]);
                if (!isExist)
                {
                    Console.WriteLine("Файл не найден: " + paths[i]);

                    if (isStrict)
                    {
                        return isExist;
                    }
                    else
                    {
                        paths[i] = null;
                    }
                }
            }
            return true;
        }
    }
}

