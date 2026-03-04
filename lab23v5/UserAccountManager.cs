public class UserAccountManager
{
    private readonly IUserRepository _repository;
    private readonly IEmailSender _emailSender;
    private readonly ISmsSender _smsSender;

    public UserAccountManager(
        IUserRepository repository,
        IEmailSender emailSender,
        ISmsSender smsSender)
    {
        _repository = repository;
        _emailSender = emailSender;
        _smsSender = smsSender;
    }

    public void Register(string email, string password)
    {
        _repository.SaveUser(email, password);
        _emailSender.SendEmail(email, "Registration successful.");
        _smsSender.SendSms("User registered successfully.");
    }

    public void Login(string email, string password)
    {
        bool isValid = _repository.ValidateUser(email, password);

        if (isValid)
        {
            _emailSender.SendEmail(email, "Login successful.");
        }
        else
        {
            _smsSender.SendSms("Login failed.");
        }
    }
}