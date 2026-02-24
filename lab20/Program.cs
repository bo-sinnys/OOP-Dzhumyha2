using System;
using lab20.Models;
using lab20.Interfaces;
using lab20.Services;

namespace lab20
{
    class Program
    {
        static void Main(string[] args)
        {
            IOrderValidator validator = new OrderValidator();
            IOrderRepository repository = new InMemoryOrderRepository();
            IEmailService emailService = new ConsoleEmailService();

            OrderService orderService = new OrderService(
                validator,
                repository,
                emailService);

            Order validOrder = new Order(1, "Anton Petrenko", 1000m);
            orderService.ProcessOrder(validOrder);
            Console.WriteLine($"Final Status: {validOrder.Status}");

            Order invalidOrder = new Order(2, "Masha Ivanova", 0m);
            orderService.ProcessOrder(invalidOrder);
            Console.WriteLine($"Final Status: {invalidOrder.Status}");
        }
    }
}