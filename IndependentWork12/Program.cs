using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Program
{
    /*
    ==========================================================
    ЗВІТ ДО САМОСТІЙНОЇ РОБОТИ №12
    Тема: PLINQ: дослідження продуктивності та безпеки
    ==========================================================

    Мета роботи:
    Дослідити продуктивність PLINQ у порівнянні зі звичайним LINQ,
    а також продемонструвати можливі проблеми потокобезпечності
    при паралельній обробці даних.

    Проведені експерименти:
    Було створено великі колекції випадкових чисел:
    - 1 000 000 елементів
    - 5 000 000 елементів
    - 10 000 000 елементів

    Для кожного елемента виконувалась обчислювально інтенсивна
    операція (математичне обчислення з використанням sqrt та pow).

    Продуктивність вимірювалась за допомогою Stopwatch.
    */

    static void Main()
    {
        Console.WriteLine("PLINQ Performance Test\n");

        RunExperiment(1_000_000);
        RunExperiment(5_000_000);
        RunExperiment(10_000_000);

        Console.WriteLine("\n=============================");
        Console.WriteLine("Side Effect Demonstration");
        Console.WriteLine("=============================\n");

        SideEffectProblem();
        SideEffectFixed();
    }

    static void RunExperiment(int size)
    {
        Console.WriteLine($"Dataset size: {size}");

        Random random = new Random();

        List<int> numbers = new List<int>(size);

        for (int i = 0; i < size; i++)
        {
            numbers.Add(random.Next(1, 10000));
        }

        Stopwatch stopwatch = new Stopwatch();

        /*
        =====================
        LINQ TEST
        =====================
        */

        stopwatch.Start();

        var linqResult = numbers
            .Where(n => HeavyOperation(n) > 100)
            .Select(n => HeavyOperation(n))
            .ToList();

        stopwatch.Stop();

        Console.WriteLine($"LINQ time: {stopwatch.ElapsedMilliseconds} ms");

        /*
        =====================
        PLINQ TEST
        =====================
        */

        stopwatch.Restart();

        var plinqResult = numbers
            .AsParallel()
            .Where(n => HeavyOperation(n) > 100)
            .Select(n => HeavyOperation(n))
            .ToList();

        stopwatch.Stop();

        Console.WriteLine($"PLINQ time: {stopwatch.ElapsedMilliseconds} ms");

        Console.WriteLine();
    }

    /*
    Обчислювально інтенсивна операція
    */

    static double HeavyOperation(int value)
    {
        double result = value;

        for (int i = 0; i < 50; i++)
        {
            result = Math.Sqrt(result * result + 10);
        }

        return result;
    }

    /*
    ==========================================================
    ПРОБЛЕМА ПОБІЧНИХ ЕФЕКТІВ
    ==========================================================

    Тут ми змінюємо спільну змінну з декількох потоків.
    Це може призвести до неправильних результатів.
    */

    static void SideEffectProblem()
    {
        List<int> numbers = Enumerable.Range(1, 1_000_000).ToList();

        int counter = 0;

        numbers
            .AsParallel()
            .ForAll(n =>
            {
                counter++; // НЕ потокобезпечна операція
            });

        Console.WriteLine($"Incorrect counter result: {counter}");
    }

    /*
    ==========================================================
    ВИПРАВЛЕННЯ ПРОБЛЕМИ
    ==========================================================

    Використання lock для потокобезпечної операції.
    */

    static void SideEffectFixed()
    {
        List<int> numbers = Enumerable.Range(1, 1_000_000).ToList();

        int counter = 0;

        object locker = new object();

        numbers
            .AsParallel()
            .ForAll(n =>
            {
                lock (locker)
                {
                    counter++;
                }
            });

        Console.WriteLine($"Correct counter result: {counter}");
    }

    /*
    ==========================================================
    ВИСНОВКИ
    ==========================================================

    1. PLINQ дозволяє виконувати операції паралельно на декількох
       потоках, що може значно підвищити продуктивність.

    2. Найбільший виграш у швидкості спостерігається при
       великих обсягах даних та обчислювально складних операціях.

    3. Для малих колекцій PLINQ може працювати повільніше через
       накладні витрати на створення потоків.

    4. Використання PLINQ може призвести до проблем з
       потокобезпечністю при модифікації спільних змінних.

    5. Для уникнення проблем необхідно використовувати
       механізми синхронізації (lock) або потокобезпечні структури.

    Отже, PLINQ є потужним інструментом для паралельної обробки
    даних, але його потрібно використовувати обережно,
    враховуючи особливості багатопоточності.
    */
}