using GymPassLab.Interfaces;

namespace GymPassLab.Services
{
    public class GymService
    {
        private readonly IGymPassStrategy _strategy;

        public GymService(IGymPassStrategy strategy)
        {
            _strategy = strategy;
        }

        public decimal Calculate(int hours, bool sauna, bool pool)
        {
            return _strategy.CalculateCost(hours, sauna, pool);
        }
    }
}