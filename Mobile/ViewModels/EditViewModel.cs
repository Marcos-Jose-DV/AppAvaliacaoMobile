using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mobile.Models;
using Mobile.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mobile.ViewModels;

public partial class EditViewModel : ObservableObject, IQueryAttributable
{
    private readonly IMovieService _movieService;

    [ObservableProperty]
    Movie _Movie;


    [ObservableProperty]
    Image _image = new();

    public EditViewModel(IMovieService movieService)
    {
        _movieService = movieService;
    }



    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var data = query["Data"];

        if (data is Movie movie)
        {
            Movie = movie;
            Image.Source = ImageSource.FromUri(new Uri(Movie.Image));
        }
    }
    [RelayCommand]
    async void Edit()
    {

        var result = await App.Current.MainPage.DisplayAlert("Editar", "Confimar alterações", "Sim", "Não");
        
        if (result)
        {
            if (Movie != null && Movie is Movie movie)
            {
                await _movieService.PutMovie(movie.Id, movie);
                Movie = null;
            }

            WeakReferenceMessenger.Default.Send<string>(new string("Load"));
            await Shell.Current.GoToAsync("..");
        }
    }

    [RelayCommand]
    async void FileUpload()
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Selecione o image",
            FileTypes = FilePickerFileType.Images
        });

        if (result is null) return;

        var stream = await result.OpenReadAsync();
        var image = await result.OpenReadAsync();

        Image.Source = ImageSource.FromStream(()=> stream);
        Movie.Image = await ConvertImageToBase64String(image);
    }

    private async Task<string> ConvertImageToBase64String(Stream stream)
    {
        byte[] bytes;
        using (MemoryStream ms = new MemoryStream())
        {
            await stream.CopyToAsync(ms);
            bytes = ms.ToArray();
        }

        var base64Image = Convert.ToBase64String(bytes);
        return base64Image;
    }
}
