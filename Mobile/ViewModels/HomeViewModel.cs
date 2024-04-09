using ClosedXML.Excel;
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

        // Escreva os dados em um arquivo de texto
        //using (StreamWriter writer = new StreamWriter("C:\\Users\\marco\\Downloads\\movies.txt"))
        //{
        //    foreach (var movie in Assessments)
        //    {
        //        writer.WriteLine($"Id: {movie.Id}, Title: {movie.Name}, Director: {movie.Director}");
        //    }
        //}


        //using (var workbook = new XLWorkbook())
        //{
        //    var worksheet = workbook.Worksheets.Add("Assessments");

        //    // Escreve os cabeçalhos das colunas
        //    worksheet.Cell(1, 1).Value = "Id";
        //    worksheet.Cell(1, 2).Value = "Category";
        //    worksheet.Cell(1, 3).Value = "Title";
        //    worksheet.Cell(1, 4).Value = "Director";
        //    worksheet.Cell(1, 5).Value = "ImageUrl";
        //    worksheet.Cell(1, 6).Value = "Gender";
        //    worksheet.Cell(1, 7).Value = "Duration";
        //    worksheet.Cell(1, 8).Value = "Concluded";
        //    worksheet.Cell(1, 9).Value = "Comments";
        //    worksheet.Cell(1, 10).Value = "Created";

        //    // Escreve os dados
        //    int row = 2;
        //    foreach (var assessment in Assessments)
        //    {
        //        worksheet.Cell(row, 1).Value = assessment.Id;
        //        worksheet.Cell(row, 2).Value = assessment.Category;
        //        worksheet.Cell(row, 3).Value = assessment.Name;
        //        worksheet.Cell(row, 4).Value = assessment.Director;
        //        worksheet.Cell(row, 5).Value = assessment.Image;
        //        worksheet.Cell(row, 6).Value = assessment.Gender;
        //        worksheet.Cell(row, 7).Value = assessment.Duration;
        //        worksheet.Cell(row, 8).Value = assessment.Concluded;
        //        worksheet.Cell(row, 9).Value = assessment.Comments;
        //        worksheet.Cell(row, 10).Value = assessment.Created;
        //        row++;
        //    }

        //    // Salva o arquivo Excel
        //    string filePath = "C:\\Users\\marco\\Downloads\\movies.xlsx";
        //    workbook.SaveAs(filePath);
        //}
    }
}
