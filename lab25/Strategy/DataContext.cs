namespace lab25.Strategy
{
    public class DataContext
    {
        private IDataProcessorStrategy _strategy;

        public DataContext(IDataProcessorStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IDataProcessorStrategy strategy)
        {
            _strategy = strategy;
        }

        public string Execute(string data)
        {
            return _strategy.Process(data);
        }
    }
}