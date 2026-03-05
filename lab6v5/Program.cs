using System;

using System.Collections.Generic;
using System.Linq;

delegate int Operation(int a, int b);

class Employee
{
    public string Name { get; set; }
    public string Position { get; set; }
    public double Salary { get; set; }

    public Employee(string name, string position, double salary)
    {
        Name = name;
        Position = position;
        Salary = salary;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Lab 6: Lambda & Delegates ===\n");

        Operation add = delegate (int x, int y)
        {
            return x + y;
        };

        Console.WriteLine("Анонімний метод (add 5+7): " + add(5, 7));

        Operation multiply = (x, y) => x * y;
        Console.WriteLine("Лямбда multiply 5*7: " + multiply(5, 7));

        List<Employee> staff = new List<Employee>()
        {
            new Employee("Іван", "Менеджер", 12000),
            new Employee("Олег", "Програміст", 18000),
            new Employee("Анна", "HR", 9000),
            new Employee("Марія", "Дизайнер", 15000),
            new Employee("Степан", "Технік", 8000)
        };

        Predicate<Employee> highSalaryCheck = emp => emp.Salary > 10000;
 
        var highSalaryEmployees = staff.Where(emp => highSalaryCheck(emp));

        Func<Employee, bool> funcHighSalary = e => e.Salary > 10000;

        var highSalaryEmployees2 = staff.Where(funcHighSalary);

        Action<Employee> printEmp = e =>
            Console.WriteLine($"{e.Name} - {e.Position}, зарплата {e.Salary}");

        Console.WriteLine("\nПрацівники із зарплатою > 10000:");
        foreach (var emp in highSalaryEmployees2)
            printEmp(emp);

        var sorted = staff.OrderBy(e => e.Salary);
        Console.WriteLine("\nВідсортовано за зарплатою:");
        foreach (var emp in sorted) printEmp(emp);

        var names = staff.Select(e => e.Name);
        Console.WriteLine("\nІмена працівників: " + string.Join(", ", names));

        double totalSalary = staff.Aggregate(0.0, (sum, e) => sum + e.Salary);
        Console.WriteLine($"\nЗагальна сума зарплат: {totalSalary}");

        Console.WriteLine("\n--- Кінець виконання ---");
    }
}