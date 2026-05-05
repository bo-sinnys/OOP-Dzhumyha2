namespace lab30v5;

public static class MathHelper
{
    /// <summary>
    /// Перевіряє, чи є число простим.
    /// Просте число — натуральне число > 1, яке ділиться лише на 1 і на себе.
    /// </summary>
    public static bool IsPrime(int n)
    {
        if (n < 2) return false;
        if (n == 2) return true;
        if (n % 2 == 0) return false;
        for (int i = 3; i * i <= n; i += 2)
            if (n % i == 0) return false;
        return true;
    }

    /// <summary>
    /// Обчислює факторіал числа n (n!).
    /// Визначено для n >= 0. 0! = 1.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Якщо n < 0.</exception>
    public static long Factorial(int n)
    {
        if (n < 0)
            throw new ArgumentOutOfRangeException(nameof(n), "Факторіал визначено лише для невід'ємних чисел.");
        if (n > 20)
            throw new ArgumentOutOfRangeException(nameof(n), "n не повинно перевищувати 20 (переповнення long).");
        long result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;
        return result;
    }

    /// <summary>
    /// Повертає n-е число Фібоначчі (0-індексація: F(0)=0, F(1)=1, F(2)=1, ...).
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Якщо n < 0.</exception>
    public static long Fibonacci(int n)
    {
        if (n < 0)
            throw new ArgumentOutOfRangeException(nameof(n), "Індекс Фібоначчі не може бути від'ємним.");
        if (n == 0) return 0;
        if (n == 1) return 1;
        long prev = 0, curr = 1;
        for (int i = 2; i <= n; i++)
            (prev, curr) = (curr, prev + curr);
        return curr;
    }

    /// <summary>
    /// Знаходить найбільший спільний дільник двох чисел (алгоритм Евкліда).
    /// GCD завжди невід'ємний. GCD(0, 0) = 0.
    /// </summary>
    public static int GCD(int a, int b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (b != 0)
            (a, b) = (b, a % b);
        return a;
    }
}
