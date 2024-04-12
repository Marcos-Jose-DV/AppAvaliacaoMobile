using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Irony.Parsing;
using Microsoft.Maui.Graphics.Text;
using Mobile.Models;
using Mobile.Services.Interfaces;
using Mobile.Views;
using System.Collections.ObjectModel;

namespace Mobile.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IAssessmentService _assessmentService;

    [ObservableProperty]
    ObservableCollection<Assessments> _assessments;

    [ObservableProperty]
    List<Assessments> _assessmentsAll;

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

            if (update != -1 && update != 0)
            {
                AssessmentsAll[update] = msg;
            }
            else
            {
                AssessmentsAll.Add(msg);
            }
            FilterAssessments();
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
        Assessments = null;
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
            Assessments = null;
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
            Assessments = null;
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

        Dictionary<string, string> categories = new()
        {
            { "Livros", "Book" },
            { "Séries", "Série" },
            { "Filmes", "Movie" },
            { "Músicas", "Music" },
        };

        if (main.Equals("Inicio"))
        {
            var queryGoup = AssessmentsAll
                      .GroupBy(a => a.Category)
                      .SelectMany(group => group
                      .OrderByDescending(a => a.Name)
                      .Take(8)
                      .ToList());
            Assessments = new ObservableCollection<Assessments>(queryGoup);
            return;
        }

        var query = AssessmentsAll
                .Where(book => book.Category == categories[main])
                .OrderByDescending(x => x.Created)
                .ToList();
        Assessments = new ObservableCollection<Assessments>(query);
    }
    [RelayCommand]
    void Filter(string filter)
    {
        string title = Shell.Current.CurrentItem.Title;

        if (title.Equals("Inicio"))
        {
            var queryGoup = AssessmentsAll
                       .GroupBy(a => a.Category)
                      .SelectMany(group => group
                      .OrderByDescending(a => a.Created)
                      .ToList());

            Assessments = new ObservableCollection<Assessments>(queryGoup);
            return;
        }

        Dictionary<string, string> categories = new()
        {
            { "Livros", "Book" },
            { "Séries", "Série" },
            { "Filmes", "Movie" },
            { "Músicas", "Music" },
        };

        Dictionary<string, bool> concluid = new()
        {
            { "True", true},
            { "False", false }
        };

        var query = AssessmentsAll.Where(x => x.Category == categories[title]);

        if (filter.Equals("Mais"))
        {
            var filters = query
                .OrderByDescending(x => x.Created)
                .ToList();

            Assessments = new ObservableCollection<Assessments>(filters);
        }
        else if (filter.Equals("Menos"))
        {
            var filters = query
               .OrderBy(x => x.Created)
                .ToList();

            Assessments = new ObservableCollection<Assessments>(filters);
        }
        else if (filter.Equals("True") || filter.Equals("False"))
        {
            var filters = query
                .Where(x => x.Concluded == concluid[filter])
                .OrderByDescending(x => x.Created)
                .ToList();

            Assessments = new ObservableCollection<Assessments>(filters);
        }
        else if (filter.Equals("Maior"))
        {
            var filters = query
                   .OrderByDescending(x => int.Parse(x.Assessment))
                    .ToList();

            Assessments = new ObservableCollection<Assessments>(filters);
        }
        else if (filter.Equals("Menor"))
        {
            var filters = query
                .OrderBy(x => int.Parse(x.Assessment))
                .ToList();

            Assessments = new ObservableCollection<Assessments>(filters);
        }
        else
        {
            var filters = query
               .OrderByDescending(x => x.Created)
                .ToList();

            Assessments = new ObservableCollection<Assessments>(filters);
        }
    }

    [RelayCommand]
    async Task Reload()
    {
        var queryGoup = AssessmentsAll
                     .GroupBy(a => a.Category)
                     .SelectMany(group => group
                     .OrderByDescending(a => a.Name)
                     .Take(8)
                     .ToList());
        Assessments = new ObservableCollection<Assessments>(queryGoup);
    }
}
