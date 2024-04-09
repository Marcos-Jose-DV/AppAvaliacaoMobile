using CommunityToolkit.Mvvm.ComponentModel;
using Mobile.Models;
namespace Mobile.ViewModels;

public partial class DetailsViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    Assessments _assessment;
    public DetailsViewModel()
    {

    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var data = (Assessments)query["Data"];
        Assessment = data;
    }
}
