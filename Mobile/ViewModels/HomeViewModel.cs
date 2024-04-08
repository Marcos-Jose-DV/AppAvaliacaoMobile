using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mobile.Models;
using Mobile.Services.Interfaces;
using System.Security.Cryptography;
using System.Windows.Input;

namespace Mobile.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IAssessmentService _assessmentService;


    [ObservableProperty]
    public IEnumerable<Assessments> _assessments;

    [ObservableProperty]
    bool _showPrevie;

    [ObservableProperty]
    string _priveiTitle;
    public HomeViewModel(IAssessmentService assessmentService)
    {
        _assessmentService = assessmentService;
        ShowPrevie = false;
        PriveiTitle = "👁️";
        LoadMovie();


        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) =>
        {
            if (msg.Equals("Load"))
            {
                Load();
            }
        });
    }

    [RelayCommand]
    async void Load()
    {
        LoadMovie();
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
            await _assessmentService.DeleteAssessment(id);
            LoadMovie();
        }
    }
    private async void LoadMovie()
    {
        var assessments = await _assessmentService.GetAssessments();
        Assessments = assessments
                    .GroupBy(a => a.Category)
                    .SelectMany(group => group.OrderByDescending(a => a.Id).Take(4))
                    .ToList();
    }
}
