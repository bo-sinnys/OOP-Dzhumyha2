namespace lab25.FactoryMethod
{
    public class FileLoggerFactory : LoggerFactory
    {
        public override ILogger CreateLogger()
        {
            return new FileLogger();
        }
    }
}