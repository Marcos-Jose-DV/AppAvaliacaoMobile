namespace Models.Models.ViewModels;

public class AssessmentsVieModel
{
    public string Name { get; set; } = string.Empty;
    public string Assessment { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int Duration { get; set; }
    public bool Concluded { get; set; }
    public string Category { get; set; } = string.Empty;
}
