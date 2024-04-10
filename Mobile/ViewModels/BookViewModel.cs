using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mobile.Models;
using Mobile.Services.Interfaces;

namespace Mobile.ViewModels;

public partial class BookViewModel : ObservableObject
{
    private readonly IAssessmentService _services;

    [ObservableProperty]
    IEnumerable<Assessments> _assessments;

    [ObservableProperty]
    bool _showPrevie;

    [ObservableProperty]
    string _priveiTitle;

    public BookViewModel(IAssessmentService services)
    {
        _services = services;
        ShowPrevie = false;
        PriveiTitle = "👁️";
        Load();


        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) =>
        {
            if (msg.Equals("Load"))
            {
                Load();
            }
        });
    }
    [RelayCommand]
    async void Previe()
    {
        ShowPrevie = !ShowPrevie;

        if (ShowPrevie)
        {
            PriveiTitle = "👁️‍🗨️";
            return;
        }

        PriveiTitle = "👁️";
    }

    [RelayCommand]
    async void Detail(object data)
    {
        var parameter = new ShellNavigationQueryParameters
        {
            { "Data", data }
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
    async void Edit(object data)
    {
        var parameter = new ShellNavigationQueryParameters
        {
            { "Data", data }
        };

        try
        {
            await Shell.Current.GoToAsync($"EditPage", parameter);
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
            await _services.DeleteAssessment(id);
            Load();
        }
    }
    private async void Load()
    {

        var main = Shell.Current.CurrentItem.Title;
        
        var assessmentsBooks = await _services.GetAssessments();

        Assessments = assessmentsBooks
            .Where(book => book.Category == "Book")
            .OrderBy(x => x.Created)
            .ToList();
    }
}
