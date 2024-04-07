using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mobile.Models;
using Mobile.Services.Interfaces;
using Mobile.Views;
using System;
using System.Windows.Input;
namespace Mobile.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IMovieService _movieService;

    [ObservableProperty]
    public IEnumerable<Movie> _movies;

    public HomeViewModel(IMovieService movieService)
    {
        _movieService = movieService;
        LoadMovie();
    }

    [RelayCommand]
    async void Load()
    {
        LoadMovie();
    }

    [RelayCommand]
    async void Detail(Movie movie)
    {
        var parameter = new ShellNavigationQueryParameters
        {
            { "Data", movie }
        };
      
        try
        {
            await Shell.Current.GoToAsync($"DetailsPage", parameter);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message} - {ex.StackTrace}");
        }
    }

    [RelayCommand]
    async void Delete(int id)
    {
        var result = await App.Current.MainPage.DisplayAlert("Remove", "Remover essa avaliação?", "Sim", "Não");
        if (result)
        {
            await _movieService.DeleteMovie(id);
            LoadMovie();
        }
    }
    private async void LoadMovie()
    {
        var movies = await _movieService.GetMovies();
        Movies = movies.OrderByDescending(movie => movie.Assessment).Take(4);
    }

}
