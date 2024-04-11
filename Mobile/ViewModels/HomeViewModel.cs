using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mobile.Models;
using Mobile.Services.Interfaces;
using Mobile.Views;

namespace Mobile.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IAssessmentService _assessmentService;

    [ObservableProperty]
    public IEnumerable<Assessments> _assessments, _assessmentsBooks, _assessmentsSeries;

    [ObservableProperty]
    public List<Assessments> _assessmentsAll;

    [ObservableProperty]
    bool _showPrevie;

    [ObservableProperty]
    string _priveiTitle;
    public HomeViewModel(IAssessmentService assessmentService)
    {
        _assessmentService = assessmentService;

        WeakReferenceMessenger.Default.Register<Assessments>(this, (e, msg) =>
        {
            var update = AssessmentsAll.FindIndex(item => item.Id == msg.Id);

            if (update != -1)
            {
                AssessmentsAll[update] = msg;
                FilterAssessments();
            }
        });
    }


    [RelayCommand]
    async Task Previe()
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
    async Task Delete(int id)
    {
        var result = await App.Current.MainPage.DisplayAlert("Remove", "Remover essa avaliação?", "Sim", "Não");
        if (result)
        {
            await _assessmentService.DeleteAssessment(id);
            Load();
        }
    }
    public async Task Load()
    {
        ShowPrevie = false;
        PriveiTitle = "👁️";
        Assessments = null;

        try
        {
            if (AssessmentsAll is null)
                AssessmentsAll = (List<Assessments>)await _assessmentService.GetAssessments();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

        FilterAssessments();
    }
    void FilterAssessments()
    {
        var main = Shell.Current.CurrentItem.Title;
        switch (main)
        {
            case "Inicio":
                Assessments = AssessmentsAll
                          .GroupBy(a => a.Category)
                          .SelectMany(group => group.OrderByDescending(a => a.Id).Take(4))
                          .ToList();
                break;
            case "Livros":
                Assessments = AssessmentsAll
                        .Where(book => book.Category == "Book")
                        .OrderBy(x => x.Created)
                        .ToList();
                break;
            case "Séries":
                Assessments = AssessmentsAll
                        .Where(book => book.Category == "Série")
                        .OrderBy(x => x.Created)
                        .ToList();
                break;
            case "Filmes":
                Assessments = AssessmentsAll
                        .Where(book => book.Category == "Movie")
                        .OrderBy(x => x.Created)
                        .ToList();
                break;
            default:
                Assessments = AssessmentsAll
                        .Where(book => book.Category == "Music")
                        .OrderBy(x => x.Created)
                        .ToList();
                break;
        };
    }
}

