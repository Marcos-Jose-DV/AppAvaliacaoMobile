using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mobile.Models;
using Mobile.Services.Interfaces;
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
        Movies = await _movieService.GetMovies();
    }

    [RelayCommand]
    void Delete(int id)
    {
        _movieService.DeleteMovie(id);
    }
    private async void LoadMovie()
    {
        var movies = await _movieService.GetMovies();
        Movies = movies;
    }

}
