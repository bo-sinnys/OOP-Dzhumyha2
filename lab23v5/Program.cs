using System;

class Program
{
    static void Main(string[] args)
    {
        IUserRepository repository = new DatabaseConnection();
        IEmailSender emailSender = new SmtpClientService();
        ISmsSender smsSender = new SmsGatewayService();

        UserAccountManager manager =
            new UserAccountManager(repository, emailSender, smsSender);

        manager.Register("user@example.com", "1234");
        manager.Login("user@example.com", "1234");

        Console.ReadLine();
    }
}