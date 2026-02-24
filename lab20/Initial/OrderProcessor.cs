using System;
using lab20.Models;

namespace lab20.Initial
{
    public class OrderProcessor
    {
        public void ProcessOrder(Order order)
        {
            Console.WriteLine("Processing order...");

            if (order.TotalAmount <= 0)
            {
                Console.WriteLine("Validation failed. TotalAmount must be > 0.");
                order.Status = OrderStatus.Cancelled;
                return;
            }

            Console.WriteLine($"Saving order {order.Id} to database...");

            Console.WriteLine($"Sending email to {order.CustomerName}...");

            order.Status = OrderStatus.Processed;

            Console.WriteLine("Order processed successfully.");
        }
    }
}