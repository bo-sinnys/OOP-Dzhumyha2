namespace lab24.Strategy
{
    public class CubeOperationStrategy : INumericOperationStrategy
    {
        public string OperationName => "Cube";

        public double Execute(double value)
        {
            return value * value * value;
        }
    }
}