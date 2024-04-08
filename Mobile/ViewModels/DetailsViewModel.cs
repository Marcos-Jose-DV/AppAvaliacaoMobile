using CommunityToolkit.Mvvm.ComponentModel;
using Mobile.Models;
using System;
namespace Mobile.ViewModels;

public partial class DetailsViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    object _Data;


    [ObservableProperty]
    string _name, _pageOrDuration, _pageOrDurationTitle, _authorOrDirecto, _authorOrDirectoTilte;
    public DetailsViewModel()
    {

    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var data = query["Data"];

        if (data is Book book)
        {
            LoadDetails(book, book.BookName, book.Pages.ToString(), "Paginas: ", book.Autor, "Autor");
        }
        else if (data is Movie movie)
        {
            LoadDetails(movie, movie.MovieName, movie.Duration.ToString(), "Duração: ", movie.Director, "Diretor");
        }
    }
    private void LoadDetails(
        object data,
        string name,
        string pageOrDuration,
        string pageOrDurationTitle,
        string authorOrDirecto,
        string authorOrDirectoTilte)
    {
        Data = data;
        Name = name;
        PageOrDuration = pageOrDuration;
        PageOrDurationTitle = pageOrDurationTitle;
        AuthorOrDirecto = authorOrDirecto;
        AuthorOrDirectoTilte = authorOrDirectoTilte;
    }
}
