using System;

public class DatabaseConnection : IUserRepository
{
    public void SaveUser(string email, string password)
    {
        Console.WriteLine($"[Database] User {email} saved.");
    }

    public bool ValidateUser(string email, string password)
    {
        Console.WriteLine($"[Database] User {email} validated.");
        return true;
    }
}