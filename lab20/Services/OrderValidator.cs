using System;
using lab20.Interfaces;
using lab20.Models;

namespace lab20.Services
{
    public class OrderValidator : IOrderValidator
    {
        public bool IsValid(Order order)
        {
            if (order.TotalAmount <= 0)
            {
                Console.WriteLine("Validation failed: TotalAmount must be > 0.");
                return false;
            }

            return true;
        }
    }
}