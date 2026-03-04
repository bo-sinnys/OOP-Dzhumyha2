using lab25.Singleton;

namespace lab25.Observer
{
    public class ProcessingLoggerObserver
    {
        public void Subscribe(DataPublisher publisher)
        {
            publisher.DataProcessed += OnDataProcessed;
        }

        private void OnDataProcessed(string data)
        {
            var logger = LoggerManager.Instance.GetLogger();
            logger.Log($"Processed data: {data}");
        }
    }
}