namespace Mobile.Models;

public class Movie
{
    public int Id { get; set; }
    public string MovieName { get; set; } = string.Empty;
    public string Assessment { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public double Duration { get; set; }
    public bool Concluded { get; set; }
    public string Comments { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}