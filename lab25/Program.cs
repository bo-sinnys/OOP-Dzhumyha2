using System;
using lab25.FactoryMethod;
using lab25.Singleton;
using lab25.Strategy;
using lab25.Observer;

namespace lab25
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== SCENARIO 1: FULL INTEGRATION =====");

            LoggerManager.Instance.SetFactory(new ConsoleLoggerFactory());

            var context = new DataContext(new EncryptDataStrategy());
            var publisher = new DataPublisher();
            var observer = new ProcessingLoggerObserver();
            observer.Subscribe(publisher);

            string result1 = context.Execute("Hello World");
            publisher.Publish(result1);

            Console.WriteLine("\n===== SCENARIO 2: CHANGE LOGGER =====");

            LoggerManager.Instance.SetFactory(new FileLoggerFactory());

            string result2 = context.Execute("Second Run");
            publisher.Publish(result2);

            Console.WriteLine("Check log.txt for file logging.");

            Console.WriteLine("\n===== SCENARIO 3: CHANGE STRATEGY =====");

            context.SetStrategy(new CompressDataStrategy());

            string result3 = context.Execute("Compressed Data Example");
            publisher.Publish(result3);

            Console.WriteLine("Finished.");
        }
    }
}