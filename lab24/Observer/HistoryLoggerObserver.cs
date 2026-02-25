using System.Collections.Generic;

namespace lab24.Observer
{
    public class HistoryLoggerObserver
    {
        public List<string> History { get; } = new List<string>();

        public void Subscribe(ResultPublisher publisher)
        {
            publisher.ResultCalculated += OnResultCalculated;
        }

        private void OnResultCalculated(double result, string operation)
        {
            History.Add($"Operation: {operation}, Result: {result}");
        }
    }
}