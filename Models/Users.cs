using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace fareShare.Models;

public class User
{
    [JsonIgnore]
    public int UserId { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
