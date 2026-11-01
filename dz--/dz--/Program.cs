using System;
using System.Text;

public class Fraction
{
    private long numerator;
    private long denominator;

    public long Numerator
    {
        get { return numerator; }
        set { numerator = value; }
    }

    public long Denominator
    {
        get { return denominator; }
        set
        {
            if (value == 0)
                throw new ArgumentException("Знаменник не може дорівнювати нулю.");
            denominator = value;
        }
    }

    public Fraction(long numerator, long denominator = 1)
    {
        Numerator = numerator;
        Denominator = denominator; 
    }

    public void Reduce()
    {
        if (numerator == 0)
        {
            denominator = 1;
            return;
        }

        long gcd = Gcd(Math.Abs(numerator), Math.Abs(denominator));
        numerator /= gcd;
        denominator /= gcd;

        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }
    }

    private static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            long t = a % b;
            a = b;
            b = t;
        }
        return a;
    }

    private static Fraction Create(long numerator, long denominator)
    {
        var result = new Fraction(numerator, denominator);
        result.Reduce();
        return result;
    }

    public static Fraction operator +(Fraction a, Fraction b)
    {
        return Create(a.numerator * b.denominator + b.numerator * a.denominator,
                      a.denominator * b.denominator);
    }

    public static Fraction operator -(Fraction a, Fraction b)
    {
        return Create(a.numerator * b.denominator - b.numerator * a.denominator,
                      a.denominator * b.denominator);
    }

    public static Fraction operator *(Fraction a, Fraction b)
    {
        return Create(a.numerator * b.numerator, a.denominator * b.denominator);
    }

    public static Fraction operator /(Fraction a, Fraction b)
    {
        if (b.numerator == 0)
            throw new DivideByZeroException("Ділення на дріб, що дорівнює нулю.");
        return Create(a.numerator * b.denominator, a.denominator * b.numerator);
    }

    public static bool operator ==(Fraction a, Fraction b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.numerator * b.denominator == b.numerator * a.denominator;
    }

    public static bool operator !=(Fraction a, Fraction b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        return obj is Fraction other && this == other;
    }

    public override int GetHashCode()
    {
        Fraction reduced = Create(numerator, denominator);
        return HashCode.Combine(reduced.numerator, reduced.denominator);
    }

    public override string ToString()
    {
        return denominator == 1 ? numerator.ToString() : $"{numerator}/{denominator}";
    }
}

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("=== Демонстрація ===");
        var x = new Fraction(1, 2);
        var y = new Fraction(3, 4);
        Console.WriteLine($"x = {x}, y = {y}");
        Console.WriteLine($"x + y = {x + y}");
        Console.WriteLine($"x - y = {x - y}");
        Console.WriteLine($"x * y = {x * y}");
        Console.WriteLine($"x / y = {x / y}");

        var z = new Fraction(6, 8);
        Console.WriteLine($"\nДріб до скорочення: {z}");
        z.Reduce();
        Console.WriteLine($"Після Reduce():     {z}");

        Console.WriteLine($"\n1/2 == 2/4 ? {new Fraction(1, 2) == new Fraction(2, 4)}");
        Console.WriteLine($"1/2 != 3/4 ? {new Fraction(1, 2) != new Fraction(3, 4)}");

        try
        {
            var bad = new Fraction(1, 0);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine("\nПомилка: " + ex.Message);
        }

        Console.WriteLine("\n=== Ваші дроби ===");
        Fraction a = ReadFraction("Перший дріб");
        Fraction b = ReadFraction("Другий дріб");

        Console.WriteLine($"{a} + {b} = {a + b}");
        Console.WriteLine($"{a} - {b} = {a - b}");
        Console.WriteLine($"{a} * {b} = {a * b}");
        try
        {
            Console.WriteLine($"{a} / {b} = {a / b}");
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine("Помилка: " + ex.Message);
        }
        Console.WriteLine($"{a} == {b} ? {a == b}");
        Console.WriteLine($"{a} != {b} ? {a != b}");
    }

    private static Fraction ReadFraction(string name)
    {
        while (true)
        {
            Console.WriteLine(name + ":");
            Console.Write("  Чисельник: ");
            bool okN = long.TryParse(Console.ReadLine(), out long n);
            Console.Write("  Знаменник: ");
            bool okD = long.TryParse(Console.ReadLine(), out long d);

            if (!okN || !okD)
            {
                Console.WriteLine("Введіть цілі числа.");
                continue;
            }

            try
            {
                return new Fraction(n, d);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }
        }
    }
}