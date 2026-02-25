using System;

namespace lab24.Strategy
{
    public class SquareRootOperationStrategy : INumericOperationStrategy
    {
        public string OperationName => "Square Root";

        public double Execute(double value)
        {
            return Math.Sqrt(value);
        }
    }
}