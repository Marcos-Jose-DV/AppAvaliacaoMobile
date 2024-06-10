using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models.Models;
using Mobile.Services;
using Herpels.FileHelpers;
namespace Mobile.ViewModels;

public partial class DetailsViewModel : ObservableObject, IQueryAttributable
{
    private readonly AssessmentService _service;

    [ObservableProperty]
    Assessments _assessment = new();

    [ObservableProperty]
    IEnumerable<Assessments> _assessments;

    List<Assessments> filter = new();

    [ObservableProperty]
    bool _detailsIsVisible, _editIsVisible;

    [ObservableProperty]
    Image _image;

    [ObservableProperty]
    Stream _stringBase64;

    [ObservableProperty]
    string[] _categories;

    public DetailsViewModel(AssessmentService service)
    {
        Categories = ["Book", "Série", "Movie", "Music"];

        _service = service;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            if (query.Count == 0) return;
            var data = (Assessments)query["Data"];
            filter = [.. await _service.GetAssessmentByName($"assessment/category/{data.Category}/name/{data.Director}")];

            ConfiguretionData(data);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    private void ConfiguretionData(Assessments data)
    {
        Assessments = filter.Where(x => x.Id != data.Id).ToList();

        DisposeAsyncMemory();
        DetailsIsVisible = true;
        EditIsVisible = false;
        Assessment = data;

        if (Assessment.Image != "default.png" && Assessment.Image != "Falha interna no servidor" && Assessment.Image.Length > 0)
        {
            Image = new() { Source = ImageSource.FromUri(new Uri(Assessment.Image)) };
        }
        else
        {
            Assessment.Image = "default.png";
            Image = new() { Source = ImageSource.FromFile("default.png") };
        }
    }

    [RelayCommand]
    void Edit()
    {
        DetailsIsVisible = !DetailsIsVisible;
        EditIsVisible = !EditIsVisible;
    }

    [RelayCommand]
    async Task Save()
    {
        bool result = await App.Current.MainPage.DisplayAlert("Editar", "Confimar alterações", "Sim", "Não");

        if (result)
        {
            if (StringBase64 is not null)
            {
                Assessment.Image = await ConverterImage.ConvertImageToBase64String(StringBase64);
            }

            await _service.PutAssessment(Assessment.Id, Assessment);
            //WeakReferenceMessenger.Default.Send<Assessments>(assessment);
            await Shell.Current.GoToAsync("..");
            DisposeAsyncMemory();
        }
    }

    [RelayCommand]
    async Task Play(Assessments assessments)
    {
        var data = new Dictionary<string, object>()
        {
            {"Data", assessments},
        };
        await Shell.Current.GoToAsync($"PlayPage", data);
    }

    [RelayCommand]
    async Task Delete(int id)
    {
        var result = await App.Current.MainPage.DisplayAlert("Remove", "Remover essa avaliação?", "Sim", "Não");
        if (result)
        {
            // await _service.DeleteAssessment(id);
            await Shell.Current.GoToAsync("..");
        }
    }

    [RelayCommand]
    async Task FileUpload(CancellationToken cancellationToken)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecione o imagem",
                FileTypes = FilePickerFileType.Images
            });

            if (result is null)
            {
                await Toast.Make($"Nenhum arquivo selecionado.").Show(cancellationToken);
            }
            else
            {
                StringBase64 = await result.OpenReadAsync();
                Image = new() { Source = ImageSource.FromFile(result.FullPath) };
            };

        }
        catch (Exception ex)
        {
            await Toast.Make($"Ocorreu um erro ao selecionado o arquivo.").Show(cancellationToken);
        }
    }

    public async void DisposeAsyncMemory()
    {
        if (StringBase64 is not null)
        {
            await StringBase64.DisposeAsync();
            Assessment = null;
            filter = null;
        }
    }

    [RelayCommand]
    void SelectItem(Assessments assessments)
    {
        ConfiguretionData(assessments);
    }
}
