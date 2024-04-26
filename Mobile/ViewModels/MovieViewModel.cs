using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.Models;
using Mobile.Services.Interfaces;
using System.Collections.ObjectModel;
namespace Mobile.ViewModels;


public partial class MovieViewModel : ObservableObject
{
    private readonly IAssessmentService _assessmentService;
    [ObservableProperty]
    IEnumerable<Assessments> _assessments;

    [ObservableProperty]
    bool _showPrevie;

    [ObservableProperty]
    string _priveiTitle;

    public MovieViewModel(IAssessmentService assessmentService)
    {
        _assessmentService = assessmentService;
        Load();
    }

    async Task Load()
    {
        ShowPrevie = false;
        PriveiTitle = "👁️";
        IEnumerable<Assessments> values = await _assessmentService.GetAssessments("assessments/categories?category=Movie&skip=0");
        Assessments = new ObservableCollection<Assessments>(values.Take(15));
    }
    [RelayCommand]
    void Previe()
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
    async Task AddCard()
    {
        await Shell.Current.GoToAsync("AddCardPage");
    }

    [RelayCommand]
    async Task Detail(object data)
    {
        var parameter = new Dictionary<string, object>
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
    async Task PLay(string name)
    {
        try
        {
            await Shell.Current.GoToAsync($"PlayPage?Data={name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message} - {ex.StackTrace}");
        }
    }
    
    [RelayCommand]
    async Task Delete(int id)
    {
        var result = await App.Current.MainPage.DisplayAlert("Remove", "Remover essa avaliação?", "Sim", "Não");
        if (result)
        {
            await _assessmentService.DeleteAssessment(id);
            Load();
        }
    }
    public void Clean()
    {
        ShowPrevie = false;
        PriveiTitle = "👁️";
    }
}
