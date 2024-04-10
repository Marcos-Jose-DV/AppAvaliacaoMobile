using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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

    public DetailsViewModel(IAssessmentService service)
    {
        DetailsIsVisible = true;
        EditIsVisible = false;
        _service = service;
    }
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        DisposeAsyncMemory();

        var data = (Assessments)query["Data"];
        Assessment = data;
        Image.Source = ImageSource.FromUri(new Uri(Assessment.Image));

        DetailsIsVisible = true;
        EditIsVisible = false;
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
            Assessment.Image = await ConvertImageToBase64String(StringBase64);
            var assessment = await _service.PutAssessment(Assessment.Id, Assessment);
            WeakReferenceMessenger.Default.Send<Assessments>(assessment);
            DisposeAsyncMemory();
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

    async void DisposeAsyncMemory()
    {
        if (StringBase64 is not null)
        {
            await StringBase64.DisposeAsync();
        }
    }

    private async Task<string> ConvertImageToBase64String(Stream stream)
    {
        byte[] bytes;
        using (MemoryStream ms = new MemoryStream())
        {
            await stream.CopyToAsync(ms);
            bytes = ms.ToArray();
        }

        var base64Image = Convert.ToBase64String(bytes);
        return base64Image;
    }
}
