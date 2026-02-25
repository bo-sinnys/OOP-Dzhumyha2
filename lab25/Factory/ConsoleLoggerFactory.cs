namespace lab25.FactoryMethod
{
    public class ConsoleLoggerFactory : LoggerFactory
    {
        public override ILogger CreateLogger()
        {
            return new ConsoleLogger();
        }
    }
}