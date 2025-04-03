using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using fareShare.Migrations;
using fareShare.Models;
using fareShare.Repositories;
using Microsoft.IdentityModel.Tokens;
using Bcrypt = BCrypt.Net.BCrypt;

namespace Backend_Users.Repositories;

public class AuthService : IAuthService
{
    private static BillDbContext _context;
    private static IConfiguration _config;

    public AuthService(BillDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public User CreateUser(User user)
    {
        // TODO: Hash Password
        var passwordHash = Bcrypt.HashPassword(user.Password);
        user.Password = passwordHash;

        _context.Add(user);
        _context.SaveChanges();
        return user;
    }

    public string SignIn(string email, string password)
    {
        var user = _context.Users.SingleOrDefault(x => x.Email == email);
        var verified = false;

        if (user != null)
        {
            verified = Bcrypt.Verify(password, user.Password);
        }

        if (user == null || !verified)
        {
            return String.Empty;
        }

        // Create & return JWT
        return BuildToken(user);
    }

    private string BuildToken(User user)
    {
        var secret = _config.GetValue<String>("TokenSecret");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        // Create Signature using secret signing key
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        // Create claims to add to JWT payload
        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName ?? ""),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName ?? ""),
        };

        // Create token
        var jwt = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: signingCredentials
        );

        // Encode token
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
}
