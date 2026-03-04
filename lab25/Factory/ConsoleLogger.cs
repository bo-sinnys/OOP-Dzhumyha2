using System;

namespace lab25.FactoryMethod
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[Console] {message}");
        }
    }
}