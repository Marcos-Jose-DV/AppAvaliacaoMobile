using System.ComponentModel.DataAnnotations;

namespace Mobile.Models;

public class Assessments
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Assessment { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int Duration { get; set; }
    public bool Concluded { get; set; }
    public string Comments { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Created { get; set; }
}
