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
            bool isExistResult = true;
            for (var i = 0; i < paths.Length; i++)
            {
                bool isExistTmp = File.Exists(paths[i]);
                isExistResult = !isExistResult ? isExistResult : isExistTmp;

                if (!isExistTmp)
                {
                    Console.WriteLine("Файл не найден: " + paths[i]);
                    paths[i] = null;
                }
            }
            return isExistResult;
        }
    }
}

