using System;
using IndependentWork16.Interfaces;
using IndependentWork16.Implementations;
using IndependentWork16.Services;

namespace IndependentWork16
{
    class Program
    {
        static void Main(string[] args)
        {
            IPaymentValidator validator = new SimplePaymentValidator();
            IPaymentGateway gateway = new MockPaymentGateway();
            ITransactionLogger logger = new ConsoleTransactionLogger();
            ISmsService smsService = new MockSmsService();

            PaymentService paymentService =
                new PaymentService(validator, gateway, logger, smsService);
            paymentService.ProcessPayment("1234-5678-9999-0000", 500m);

            Console.ReadLine();
        }
    }
}