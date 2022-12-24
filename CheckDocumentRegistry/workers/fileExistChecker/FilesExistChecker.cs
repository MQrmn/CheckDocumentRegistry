namespace RegComparator
{
    public class FileExistChecker : IFileExistChecker
    {
        public void CheckCritical(string[] paths)
        {
            if (!CheckArr(paths, true))
            {
                Console.WriteLine("Завершение работы программы");
                Environment.Exit(0);
            }
        }

        public void CheckNonCritical(string[] paths)
        {
            CheckArr(paths, false);
        }

        private bool CheckArr(string[] paths, bool isStrict)
        {
            bool isExist = true;
            for (var i = 0; i < paths.Length; i++)
            {
                isExist = File.Exists(paths[i]);
                if (!isExist)
                {
                    Console.WriteLine("Файл не найден: " + paths[i]);
                    if (!isStrict)
                    {
                        paths[i] = null;
                    }
                }
            }
            return isExist;
        }
    }
}

