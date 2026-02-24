using GymPassLab.Interfaces;

namespace GymPassLab.Strategies
{
    public class FullPass : IGymPassStrategy
    {
        public decimal CalculateCost(int hours, bool sauna, bool pool)
        {
            decimal cost = hours * 15m;
            if (sauna) cost += 5m;
            if (pool) cost += 7m;
            return cost;
        }
    }
}