using fareShare.Models;

namespace fareShare.Repositories;

public interface IAuthService
{
    User CreateUser(User user);
    string SignIn(string email, string password);
    int GetUserId(string email);
    User GetUser(int userId);
    void UpdateUser(User user);
}
