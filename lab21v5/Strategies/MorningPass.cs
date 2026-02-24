using GymPassLab.Interfaces;

namespace GymPassLab.Strategies
{
    public class MorningPass : IGymPassStrategy
    {
        public decimal CalculateCost(int hours, bool sauna, bool pool)
        {
            decimal cost = hours * 10m;
            if (sauna) cost += 5m;
            if (pool) cost += 7m;
            return cost * 0.8m;
        }
    }
}