using CommunityToolkit.Mvvm.ComponentModel;
using Mobile.Models;
using System;
namespace Mobile.ViewModels;

public partial class DetailsViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    Movie _movie;

    public DetailsViewModel()
    {
        
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Movie movie = (Movie)query["Data"];
        Movie = movie;
    }
}
