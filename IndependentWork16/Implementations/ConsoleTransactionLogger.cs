using System;
using IndependentWork16.Interfaces;

namespace IndependentWork16.Implementations
{
    public class ConsoleTransactionLogger : ITransactionLogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[Logger] {message}");
        }
    }
}