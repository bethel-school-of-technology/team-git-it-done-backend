using team.Models;

namespace team.Repositories;

public interface IAuthService
{
    User CreateUser(User user);
    string SignIn(string email, string password);
}
