using System;
using lab20.Interfaces;
using lab20.Models;

namespace lab20.Services
{
    public class OrderService
    {
        private readonly IOrderValidator _validator;
        private readonly IOrderRepository _repository;
        private readonly IEmailService _emailService;

        public OrderService(
            IOrderValidator validator,
            IOrderRepository repository,
            IEmailService emailService)
        {
            _validator = validator;
            _repository = repository;
            _emailService = emailService;
        }

        public void ProcessOrder(Order order)
        {
            Console.WriteLine("\n--- Processing order ---");

            order.Status = OrderStatus.PendingValidation;

            if (!_validator.IsValid(order))
            {
                order.Status = OrderStatus.Cancelled;
                Console.WriteLine("Order cancelled.");
                return;
            }

            _repository.Save(order);
            _emailService.SendOrderConfirmation(order);

            order.Status = OrderStatus.Processed;

            Console.WriteLine("Order processed successfully.");
        }
    }
}