using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace fareShare.Models;

// Zian -> This is the Bill class that will be used to describe the bill and its properties.

public class Bill
{
    [JsonIgnore]
    public int BillId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public float? Price { get; set; }
    public int? CreatorId { get; set; }
    public ICollection<BillLink> BillLinks { get; set; }
}
