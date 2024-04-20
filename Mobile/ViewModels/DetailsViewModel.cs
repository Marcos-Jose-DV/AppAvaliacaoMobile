using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mobile.Herpels;
using Mobile.Models;
using Mobile.Services;
using Mobile.Services.Interfaces;
namespace Mobile.ViewModels;

public partial class DetailsViewModel : ObservableObject, IQueryAttributable
{
    private readonly AssessmentService _service;

    [ObservableProperty]
    Assessments _assessment = new();

    [ObservableProperty]
    bool _detailsIsVisible, _editIsVisible;

    [ObservableProperty]
    Image _image = new();

    [ObservableProperty]
    Stream _stringBase64;

    [ObservableProperty]
    string[] _categories;

    public DetailsViewModel(AssessmentService service)
    {
        Categories = ["Book", "Série", "Movie", "Music"];
        _service = service;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {

            if (query.Count == 0) return;
            var data = (Assessments)query["Data"];

            DisposeAsyncMemory();
            DetailsIsVisible = true;
            EditIsVisible = false;
            Assessment = data;
            if (Assessment.Image is null)
            {
                Image.Source = ImageSource.FromResource("dotnet_bot.png");
                return;
            }

            Image.Source = ImageSource.FromUri(new Uri(Assessment.Image));
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
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

            var assessment = await _service.PutAssessment(Assessment.Id, Assessment);
            WeakReferenceMessenger.Default.Send<Assessments>(assessment);
            await Shell.Current.GoToAsync("..");
            DisposeAsyncMemory();
        }
    }

    [RelayCommand]
    async Task Play(string name)
    {
        await Shell.Current.GoToAsync($"PlayPage?Data={name}");
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

}
