using System;
using System.Text;

namespace lab25.Strategy
{
    public class EncryptDataStrategy : IDataProcessorStrategy
    {
        public string Process(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(bytes);
        }
    }
}