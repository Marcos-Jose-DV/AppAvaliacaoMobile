using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Irony.Parsing;
using Microsoft.Maui.Graphics.Text;
using Mobile.Models;
using Mobile.Services.Interfaces;
using Mobile.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Mobile.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IAssessmentService _assessmentService;

    [ObservableProperty]
    ObservableCollection<Assessments> _assessments = new();

    [ObservableProperty]
    List<Assessments> _assessmentsAll;

    [ObservableProperty]
    List<string> _searchResults;

    [ObservableProperty]
    bool _showPrevie;

    [ObservableProperty]
    string _priveiTitle;
    public HomeViewModel(IAssessmentService assessmentService)
    {
        _assessmentService = assessmentService;

        WeakReferenceMessenger.Default.Register<Assessments>(this, (e, msg) =>
        {
            var updateIndex = Assessments.ToList().FindIndex(item => item.Id == msg.Id);

            if (updateIndex != -1)
            {
                Assessments[updateIndex] = msg;
            }
            else
            {
                Assessments.Add(msg);
            }

            Load();
        });

        Load();
    }

    [RelayCommand]
    async Task PerformSearch(string query)
    {
        var data = await _assessmentService.GetAssessmentByName($"assessment/category?category=Movie&name={query}");

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
        var query = new Dictionary<string, object>
        {
            { "Data", data }
        };

        try
        {
            await Shell.Current.GoToAsync($"DetailsPage", query);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message} - {ex.StackTrace}");
        }
    }
    public async Task Load(int skip = 0)
    {
        try
        {
            if (Assessments.Count == 34) return;

            ShowPrevie = false;
            PriveiTitle = "👁️";

            var list = await _assessmentService.GetAssessments($"assessments/recent?skip={skip}");

            if (list.Any())
            {
                foreach (var item in list)
                {
                    Assessments.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    [RelayCommand]
    void Filter(string filter)
    {
        string title = Shell.Current.CurrentItem.Title;

        if (title.Equals("Inicio"))
        {
            var queryGoup = Assessments
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

        var query = Assessments.Where(x => x.Category == categories[title]);

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
    public void Clean()
    {
        ShowPrevie = false;
        PriveiTitle = "👁️";
    }
}
