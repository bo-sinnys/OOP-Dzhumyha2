public interface IUserRepository
{
    void SaveUser(string email, string password);
    bool ValidateUser(string email, string password);
}