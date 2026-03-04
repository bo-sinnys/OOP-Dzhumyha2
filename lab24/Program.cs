using System;
using lab24.Strategy;
using lab24.Observer;

namespace lab24
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== LAB24: Strategy + Observer =====");

            var processor = new NumericProcessor(new SquareOperationStrategy());
            var publisher = new ResultPublisher();

            var consoleObserver = new ConsoleLoggerObserver();
            var historyObserver = new HistoryLoggerObserver();
            var thresholdObserver = new ThresholdNotifierObserver(50);

            consoleObserver.Subscribe(publisher);
            historyObserver.Subscribe(publisher);
            thresholdObserver.Subscribe(publisher);

            double result1 = processor.Process(5);
            publisher.PublishResult(result1, processor.GetOperationName());

            processor.SetStrategy(new CubeOperationStrategy());
            double result2 = processor.Process(4);
            publisher.PublishResult(result2, processor.GetOperationName());

            processor.SetStrategy(new SquareRootOperationStrategy());
            double result3 = processor.Process(100);
            publisher.PublishResult(result3, processor.GetOperationName());

            Console.WriteLine("\n===== History Log =====");
            foreach (var record in historyObserver.History)
            {
                Console.WriteLine(record);
            }

            Console.WriteLine("===== END =====");
        }
    }
}