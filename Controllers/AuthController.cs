using fareShare.Models;
using fareShare.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace fareShare.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, IAuthService service)
    {
        _logger = logger;
        _authService = service;
    }

    [HttpPost]
    [Route("register")]
    public ActionResult CreateUser(User user)
    {
        if (user == null || !ModelState.IsValid)
        {
            return BadRequest();
        }
        _authService.CreateUser(user);
        return NoContent();
    }

    [HttpGet]
    [Route("login")]
    public ActionResult<string> SignIn(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return BadRequest();
        }

        var token = _authService.SignIn(email, password);

        if (string.IsNullOrWhiteSpace(token))
        {
            return Unauthorized();
        }

        return Ok(token);
    }

    [HttpGet]
    [Route("user")]
    public ActionResult<int> GetUserId(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest();
        }

        var userId = _authService.GetUserId(email);

        if (userId == 0)
        {
            return NotFound();
        }

        return Ok(userId);
    }

    [HttpGet]
    [Route("user/{id}")]
    public ActionResult<User> GetUser(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        var user = _authService.GetUser(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost("update-pro-pic")]
    public ActionResult UpdateProfilePic([FromBody] User user)
    {
        var existingUser = _authService.GetUser(user.UserId);
        if (existingUser == null)
        {
            return NotFound("user not found :(");
        }
        existingUser.Img = user.Img;
        _authService.UpdateUser(existingUser);

        return Ok(new { message = "profile pic updated successfully", img = existingUser.Img });
    }
}
