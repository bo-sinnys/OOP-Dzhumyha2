    using System;

namespace lab24.Observer
{
    public class ConsoleLoggerObserver
    {
        public void Subscribe(ResultPublisher publisher)
        {
            publisher.ResultCalculated += OnResultCalculated;
        }

        private void OnResultCalculated(double result, string operation)
        {
            Console.WriteLine($"[Console] Operation: {operation}, Result: {result}");
        }
    }
}