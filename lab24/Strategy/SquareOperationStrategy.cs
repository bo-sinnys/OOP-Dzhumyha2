namespace lab24.Strategy
{
    public class SquareOperationStrategy : INumericOperationStrategy
    {
        public string OperationName => "Square";

        public double Execute(double value)
        {
            return value * value;
        }
    }
}