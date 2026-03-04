using System.IO;

namespace lab25.FactoryMethod
{
    public class FileLogger : ILogger
    {
        private readonly string _path = "log.txt";

        public void Log(string message)
        {
            File.AppendAllText(_path, $"[File] {message}{System.Environment.NewLine}");
        }
    }
}