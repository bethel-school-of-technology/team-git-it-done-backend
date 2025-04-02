using fareShare.Models;

namespace fareShare.Repositories;

public interface IAuthService
{
    User CreateUser(User user);
    string SignIn(string email, string password);
}
