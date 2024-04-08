using CommunityToolkit.Mvvm.ComponentModel;
using Mobile.Models;
namespace Mobile.ViewModels;

public partial class DetailsViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    Assessments _assessment;


    [ObservableProperty]
    string _name, _pageOrDuration, _pageOrDurationTitle, _authorOrDirecto, _authorOrDirectoTilte;
    public DetailsViewModel()
    {

    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var data = (Assessments)query["Data"];
        Assessment = data;
        if (Assessment.Category == "Book")
        {
            LoadDetails(Assessment, Assessment.Name, Assessment.Duration.ToString(), "Paginas: ", Assessment.Director, "Autor");
            return;
        }

        LoadDetails(Assessment, Assessment.Name, Assessment.Duration.ToString(), "Duração: ", Assessment.Director, "Diretor");

    }
    private void LoadDetails(
        Assessments assessment,
        string name,
        string pageOrDuration,
        string pageOrDurationTitle,
        string authorOrDirecto,
        string authorOrDirectoTilte)
    {
        Assessment = assessment;
        Name = name;
        PageOrDuration = pageOrDuration;
        PageOrDurationTitle = pageOrDurationTitle;
        AuthorOrDirecto = authorOrDirecto;
        AuthorOrDirectoTilte = authorOrDirectoTilte;
    }
}
