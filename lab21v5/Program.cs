using System;
using GymPassLab.Interfaces;
using GymPassLab.Services;
using GymPassLab.Factories;

namespace GymPassLab
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Виберіть абонемент (Morning, Day, Full):");
            string passType = Console.ReadLine();

            Console.WriteLine("Кількість годин:");
            int hours = int.Parse(Console.ReadLine());

            Console.WriteLine("Додаткові послуги: сауна (true/false):");
            bool sauna = bool.Parse(Console.ReadLine());

            Console.WriteLine("Додаткові послуги: басейн (true/false):");
            bool pool = bool.Parse(Console.ReadLine());

            IGymPassStrategy strategy = GymPassFactory.CreateStrategy(passType);
            GymService service = new GymService(strategy);

            decimal cost = service.Calculate(hours, sauna, pool);
            Console.WriteLine($"Вартість обраного абонементу: {cost} грн");
        }
    }
}