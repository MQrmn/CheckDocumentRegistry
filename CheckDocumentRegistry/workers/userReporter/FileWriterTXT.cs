using System.Text;

namespace RegComparator
{
    public class FileWriterTXT : IUserReporter
    {
        private string _path;
        public FileWriterTXT(string path) 
        {
            _path = path;
        }
        
        public void ReportError(object sender, string message)
        {
            throw new NotImplementedException();
        }

        public void ReportInfo(object sender, string message)
        {
            throw new NotImplementedException();
        }

        public void ReportSpecial(object sender, string message)
        {
            Write(message);
        }

        private void Write(string message)
        {
            using (StreamWriter writer = new StreamWriter(_path, true, Encoding.UTF8))
            {
                writer.WriteLine(message);
            }
        }
    }
}
