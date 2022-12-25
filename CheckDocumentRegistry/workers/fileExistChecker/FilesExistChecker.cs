namespace RegComparator
{
    public class FileExistChecker : IFileExistChecker
    {
        public event EventHandler<string>? ErrNotify;
        public void CheckCritical(string[] paths)
        {
            if (!CheckArr(paths, true))
            {
                ErrNotify?.Invoke(this, "Завершение работы программы");
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
                    ErrNotify?.Invoke(this, "Файл не найден: " + paths[i]);
                    paths[i] = null;
                }
            }
            return isExistResult;
        }
    }
}

