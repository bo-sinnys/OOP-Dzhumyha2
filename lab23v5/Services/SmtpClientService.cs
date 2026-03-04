using System;

public class SmtpClientService : IEmailSender
{
    public void SendEmail(string to, string message)
    {
        Console.WriteLine($"[Email] Sent to {to}: {message}");
    }
}