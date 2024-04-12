using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mobile.Herpels;
using Mobile.Models;
using Mobile.Services.Interfaces;
namespace Mobile.ViewModels;

public partial class DetailsViewModel : ObservableObject, IQueryAttributable
{
    private readonly IAssessmentService _service;

    [ObservableProperty]
    Assessments _assessment;

    [ObservableProperty]
    bool _detailsIsVisible, _editIsVisible;

    [ObservableProperty]
    Image _image = new();

    [ObservableProperty]
    Stream _stringBase64;

    [ObservableProperty]
    string[] _categories;

    public DetailsViewModel(IAssessmentService service)
    {
        Categories = ["Book", "Série", "Movie", "Music"];
        _service = service;
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count == 0)
            return;
        var data = (Assessments)query["Data"];

        DisposeAsyncMemory();
        DetailsIsVisible = true;
        EditIsVisible = false;
        Assessment = data;
        if (string.IsNullOrEmpty(Assessment.Image))
        {
            Image.Source = ImageSource.FromResource("dotnet_bot.png");
            return;
        }

        Image.Source = ImageSource.FromUri(new Uri(Assessment.Image));

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

            var assessment = await _service.PutAssessment(Assessment.Id, Assessment);
            WeakReferenceMessenger.Default.Send<Assessments>(assessment);
            await Shell.Current.GoToAsync("..");
            DisposeAsyncMemory();
        }
    }

    [RelayCommand]
    async Task FileUpload(CancellationToken cancellationToken)
    {
        try
        {
            DisposeAsyncMemory();
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecione o imagem",
                FileTypes = FilePickerFileType.Images
            });

            if (result is null) return;

            StringBase64 = await result.OpenReadAsync();
            Image.Source = ImageSource.FromFile(result.FullPath);
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
            Image = null;
            Assessment = null;
        }
    }

    [RelayCommand]
    async Task Play(string name)
    {
        await Shell.Current.GoToAsync($"PlayPage?Data={name}");
    }
}
