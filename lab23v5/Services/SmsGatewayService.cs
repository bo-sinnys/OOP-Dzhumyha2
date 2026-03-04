using System;

public class SmsGatewayService : ISmsSender
{
    public void SendSms(string message)
    {
        Console.WriteLine($"[SMS] {message}");
    }
}