using lab30v5;

namespace lab30v5.Tests;

public class IsPrimeTests
{
    // [Fact] — одиничний тест без параметрів

    [Fact]
    public void IsPrime_LessThanTwo_ReturnsFalse()
    {
        Assert.False(MathHelper.IsPrime(1));
        Assert.False(MathHelper.IsPrime(0));
        Assert.False(MathHelper.IsPrime(-7));
    }

    [Fact]
    public void IsPrime_Two_ReturnsTrue()
    {
        Assert.True(MathHelper.IsPrime(2));
    }

    [Fact]
    public void IsPrime_EvenNumberGreaterThanTwo_ReturnsFalse()
    {
        Assert.False(MathHelper.IsPrime(4));
        Assert.False(MathHelper.IsPrime(100));
    }

    // [Theory] + [InlineData] — параметризовані тести

    [Theory]
    [InlineData(2,   true)]
    [InlineData(3,   true)]
    [InlineData(5,   true)]
    [InlineData(7,   true)]
    [InlineData(11,  true)]
    [InlineData(13,  true)]
    [InlineData(97,  true)]
    [InlineData(4,   false)]
    [InlineData(9,   false)]
    [InlineData(15,  false)]
    [InlineData(1,   false)]
    [InlineData(0,   false)]
    [InlineData(-3,  false)]
    public void IsPrime_VariousInputs_ReturnsExpected(int n, bool expected)
    {
        Assert.Equal(expected, MathHelper.IsPrime(n));
    }
}

public class FactorialTests
{
    [Fact]
    public void Factorial_Zero_ReturnsOne()
    {
        Assert.Equal(1L, MathHelper.Factorial(0));
    }

    [Fact]
    public void Factorial_One_ReturnsOne()
    {
        Assert.Equal(1L, MathHelper.Factorial(1));
    }

    [Theory]
    [InlineData(2,  2L)]
    [InlineData(3,  6L)]
    [InlineData(4,  24L)]
    [InlineData(5,  120L)]
    [InlineData(10, 3628800L)]
    [InlineData(20, 2432902008176640000L)]
    public void Factorial_KnownValues_ReturnsExpected(int n, long expected)
    {
        Assert.Equal(expected, MathHelper.Factorial(n));
    }

    [Fact]
    public void Factorial_NegativeInput_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MathHelper.Factorial(-1));
    }

    [Fact]
    public void Factorial_OverTwenty_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MathHelper.Factorial(21));
    }
}

public class FibonacciTests
{
    [Theory]
    [InlineData(0,  0L)]
    [InlineData(1,  1L)]
    [InlineData(2,  1L)]
    [InlineData(3,  2L)]
    [InlineData(4,  3L)]
    [InlineData(5,  5L)]
    [InlineData(6,  8L)]
    [InlineData(7,  13L)]
    [InlineData(10, 55L)]
    [InlineData(20, 6765L)]
    public void Fibonacci_KnownValues_ReturnsExpected(int n, long expected)
    {
        Assert.Equal(expected, MathHelper.Fibonacci(n));
    }

    [Fact]
    public void Fibonacci_NegativeInput_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MathHelper.Fibonacci(-1));
    }

    [Fact]
    public void Fibonacci_SequenceProperty_EachTermIsSumOfPrevTwo()
    {
        // F(n) = F(n-1) + F(n-2) для n >= 2
        for (int i = 2; i <= 15; i++)
        {
            long fn   = MathHelper.Fibonacci(i);
            long fn1  = MathHelper.Fibonacci(i - 1);
            long fn2  = MathHelper.Fibonacci(i - 2);
            Assert.Equal(fn1 + fn2, fn);
        }
    }
}

public class GcdTests
{
    [Theory]
    [InlineData(12, 8,   4)]
    [InlineData(15, 10,  5)]
    [InlineData(100, 75, 25)]
    [InlineData(7,  3,   1)]   // взаємно прості
    [InlineData(0,  5,   5)]   // GCD(0, n) = n
    [InlineData(5,  0,   5)]
    [InlineData(0,  0,   0)]   // GCD(0, 0) = 0
    [InlineData(6,  6,   6)]   // однакові числа
    public void GCD_VariousInputs_ReturnsExpected(int a, int b, int expected)
    {
        Assert.Equal(expected, MathHelper.GCD(a, b));
    }

    [Fact]
    public void GCD_NegativeInputs_ReturnsSameAsPositive()
    {
        // GCD(-12, 8) == GCD(12, 8) == 4
        Assert.Equal(MathHelper.GCD(12, 8), MathHelper.GCD(-12, 8));
        Assert.Equal(MathHelper.GCD(12, 8), MathHelper.GCD(-12, -8));
    }

    [Fact]
    public void GCD_IsCommutative()
    {
        Assert.Equal(MathHelper.GCD(12, 8), MathHelper.GCD(8, 12));
        Assert.Equal(MathHelper.GCD(100, 75), MathHelper.GCD(75, 100));
    }

    [Fact]
    public void GCD_PrimeNumbers_ReturnsOne()
    {
        Assert.Equal(1, MathHelper.GCD(7, 11));
        Assert.Equal(1, MathHelper.GCD(13, 17));
    }
}
