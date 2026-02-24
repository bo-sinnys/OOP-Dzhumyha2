using System;
using GymPassLab.Interfaces;
using GymPassLab.Strategies;

namespace GymPassLab.Factories
{
    public static class GymPassFactory
    {
        public static IGymPassStrategy CreateStrategy(string type)
        {
            return type.ToLower() switch
            {
                "morning" => new MorningPass(),
                "day" => new DayPass(),
                "full" => new FullPass(),
                _ => throw new ArgumentException("Невідомий тип абонементу")
            };
        }
    }
}