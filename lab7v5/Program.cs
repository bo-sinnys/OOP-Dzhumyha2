using System;
using System.IO;
using System.Net.Http;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Оновлення конфігурації ===");

        var fileProcessor = new FileProcessor();
        var networkClient = new NetworkClient();

        try
        {
            // Оновлення конфігурації файлу
            RetryHelper.ExecuteWithRetry<object>(
                () =>
                {
                    fileProcessor.UpdateConfig("config.json", "version", "2.0");
                    return null;
                },
                retryCount: 5,
                initialDelay: TimeSpan.FromSeconds(1),
                shouldRetry: ex => ex is IOException
            );

            // Відправка конфігурації на сервер
            bool result = RetryHelper.ExecuteWithRetry(
                () => networkClient.SendConfigUpdate("https://api.server.com/update", "{ \"version\": \"2.0\" }"),
                retryCount: 5,
                initialDelay: TimeSpan.FromSeconds(1),
                shouldRetry: ex => ex is HttpRequestException
            );

            Console.WriteLine($"Результат відправки: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Операція остаточно завершилась помилкою: {ex.Message}");
        }

        Console.WriteLine("Роботу завершено.");
    }
}

public class FileProcessor
{
    private int attempt = 0;

    public void UpdateConfig(string path, string key, string value)
    {
        attempt++;

        Console.WriteLine($"Спроба оновлення файлу #{attempt}");

        if (attempt <= 2)
        {
            throw new IOException("Тимчасова помилка запису у файл");
        }

        Console.WriteLine($"Конфігурацію оновлено: {key} = {value}");
    }
}

public class NetworkClient
{
    private int attempt = 0;

    public bool SendConfigUpdate(string url, string configJson)
    {
        attempt++;

        Console.WriteLine($"Спроба відправки на сервер #{attempt}");

        if (attempt <= 2)
        {
            throw new HttpRequestException("Тимчасова мережева помилка");
        }

        Console.WriteLine("Конфігурацію успішно відправлено на сервер");
        return true;
    }
}

public static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool> shouldRetry = null)
    {
        if (initialDelay == default)
        {
            initialDelay = TimeSpan.FromSeconds(1);
        }

        int attempt = 0;

        while (true)
        {
            try
            {
                attempt++;
                Console.WriteLine($"Виконання операції. Спроба {attempt}");

                return operation();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");

                if (attempt >= retryCount ||
                    (shouldRetry != null && !shouldRetry(ex)))
                {
                    Console.WriteLine("Повторні спроби припинено.");
                    throw;
                }

                var delay = TimeSpan.FromMilliseconds(
                    initialDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));

                Console.WriteLine($"Очікування {delay.TotalSeconds} сек перед повтором...");

                Thread.Sleep(delay);
            }
        }
    }
}