using lab25.FactoryMethod;

namespace lab25.Singleton
{
    public sealed class LoggerManager
    {
        private static LoggerManager _instance;
        private static readonly object _lock = new object();

        private LoggerFactory _factory;

        private LoggerManager() { }

        public static LoggerManager Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new LoggerManager();
                }
            }
        }

        public void SetFactory(LoggerFactory factory)
        {
            _factory = factory;
        }

        public ILogger GetLogger()
        {
            return _factory.CreateLogger();
        }
    }
}