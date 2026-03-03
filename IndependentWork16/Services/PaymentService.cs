using IndependentWork16.Interfaces;

namespace IndependentWork16.Services
{
    public class PaymentService
    {
        private readonly IPaymentValidator _validator;
        private readonly IPaymentGateway _gateway;
        private readonly ITransactionLogger _logger;
        private readonly ISmsService _smsService;

        public PaymentService(
            IPaymentValidator validator,
            IPaymentGateway gateway,
            ITransactionLogger logger,
            ISmsService smsService)
        {
            _validator = validator;
            _gateway = gateway;
            _logger = logger;
            _smsService = smsService;
        }

        public void ProcessPayment(string cardNumber, decimal amount)
        {
            if (!_validator.Validate(cardNumber, amount))
            {
                System.Console.WriteLine("Payment validation failed.");
                return;
            }

            _gateway.Charge(cardNumber, amount);
            _logger.Log($"Transaction completed for {amount}");
            _smsService.SendSms("Your payment was successful.");
        }
    }
}