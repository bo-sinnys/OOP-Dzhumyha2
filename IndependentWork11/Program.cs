using Polly;
using Polly.Timeout;
using System;
using System.Net.Http;
using System.Threading;

class Program
{
    /*
    ==========================================================
    SCENARIO 1: External API Call
    ==========================================================

    Проблема:
    Зовнішній API може бути тимчасово недоступним через
    перевантаження або мережеві проблеми.

    Політика Polly:
    Retry з експоненційною затримкою.

    Обґрунтування:
    Retry дозволяє повторити запит кілька разів,
    якщо помилка тимчасова.

    Очікувана поведінка:
    Перші 2 виклики завершуються помилкою.
    Polly робить повторні спроби і третя спроба успішна.
    */

    private static int apiAttempts = 0;

    static string CallExternalApi()
    {
        apiAttempts++;

        Console.WriteLine($"API call attempt {apiAttempts}");

        if (apiAttempts <= 2)
        {
            throw new HttpRequestException("Temporary API failure");
        }

        return "API DATA";
    }

    /*
    ==========================================================
    SCENARIO 2: Database Connection
    ==========================================================

    Проблема:
    База даних може тимчасово не відповідати
    через перевантаження або втрату з'єднання.

    Політика Polly:
    Circuit Breaker

    Обґрунтування:
    Якщо помилки відбуваються часто,
    Circuit Breaker тимчасово блокує запити,
    щоб система не перевантажувала БД.

    Очікувана поведінка:
    Після кількох помилок ланцюг відкривається
    і запити тимчасово блокуються.
    */

    private static int dbAttempts = 0;

    static void QueryDatabase()
    {
        dbAttempts++;

        Console.WriteLine($"DB query attempt {dbAttempts}");

        if (dbAttempts <= 3)
        {
            throw new Exception("Database connection error");
        }

        Console.WriteLine("Database query successful");
    }

    /*
    ==========================================================
    SCENARIO 3: Long Operation
    ==========================================================

    Проблема:
    Деякі операції можуть виконуватися занадто довго.

    Політика Polly:
    Timeout

    Обґрунтування:
    Timeout дозволяє обмежити максимальний час
    виконання операції.

    Очікувана поведінка:
    Якщо операція триває довше ніж 2 секунди,
    Polly генерує TimeoutException.
    */

    static void LongOperation()
    {
        Console.WriteLine("Starting long operation...");

        Thread.Sleep(5000); // імітація довгої роботи

        Console.WriteLine("Operation finished");
    }

    static void Main()
    {
        Console.WriteLine("=== Polly Resilience Demo ===\n");

        /*
        ========================
        SCENARIO 1: RETRY
        ========================
        */

        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetry(
                3,
                retry => TimeSpan.FromSeconds(Math.Pow(2, retry)),
                (exception, time, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} after {time.TotalSeconds}s: {exception.Message}");
                });

        try
        {
            var result = retryPolicy.Execute(() => CallExternalApi());
            Console.WriteLine($"Final API result: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API call failed: {ex.Message}");
        }

        Console.WriteLine("\n-----------------\n");

        /*
        ========================
        SCENARIO 2: CIRCUIT BREAKER
        ========================
        */

        var circuitPolicy = Policy
            .Handle<Exception>()
            .CircuitBreaker(
                2,
                TimeSpan.FromSeconds(5),
                (ex, breakDelay) =>
                {
                    Console.WriteLine($"Circuit opened for {breakDelay.TotalSeconds}s");
                },
                () =>
                {
                    Console.WriteLine("Circuit closed again");
                });

        for (int i = 0; i < 5; i++)
        {
            try
            {
                circuitPolicy.Execute(() => QueryDatabase());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DB error: {ex.Message}");
            }

            Thread.Sleep(1000);
        }

        Console.WriteLine("\n-----------------\n");

        /*
        ========================
        SCENARIO 3: TIMEOUT
        ========================
        */

        var timeoutPolicy = Policy.Timeout(2);

        try
        {
            timeoutPolicy.Execute(() => LongOperation());
        }
        catch (TimeoutRejectedException)
        {
            Console.WriteLine("Operation timed out!");
        }

        Console.WriteLine("\n=== End of Demo ===");
    }
}