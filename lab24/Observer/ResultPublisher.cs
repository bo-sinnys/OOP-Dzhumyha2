using System;

namespace lab24.Observer
{
    public class ResultPublisher
    {
        public event Action<double, string> ResultCalculated;

        public void PublishResult(double result, string operationName)
        {
            ResultCalculated?.Invoke(result, operationName);
        }
    }
}