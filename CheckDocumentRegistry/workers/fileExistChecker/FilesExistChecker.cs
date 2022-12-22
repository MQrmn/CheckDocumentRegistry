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
            foreach (var p in paths) 
            {
                bool isExist = File.Exists(p);
                if (!isExist)
                {
                    Console.WriteLine("Файл не найден: " + p);

                    if (isStrict)
                    {
                        return isExist;
                    }
                }
            }
            return true;
        }
    }
}

