using System;
using lab20.Interfaces;
using lab20.Models;

namespace lab20.Services
{
    public class ConsoleEmailService : IEmailService
    {
        public void SendOrderConfirmation(Order order)
        {
            Console.WriteLine($"Email sent to {order.CustomerName} for order {order.Id}.");
        }
    }
}