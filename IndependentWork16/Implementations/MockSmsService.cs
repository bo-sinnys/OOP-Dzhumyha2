using System;
using IndependentWork16.Interfaces;

namespace IndependentWork16.Implementations
{
    public class MockSmsService : ISmsService
    {
        public void SendSms(string message)
        {
            Console.WriteLine($"[SMS] {message}");
        }
    }
}