using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace lab25.Strategy
{
    public class CompressDataStrategy : IDataProcessorStrategy
    {
        public string Process(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            using var output = new MemoryStream();
            using (var gzip = new GZipStream(output, CompressionMode.Compress))
            {
                gzip.Write(bytes, 0, bytes.Length);
            }
            return Convert.ToBase64String(output.ToArray());
        }
    }
}