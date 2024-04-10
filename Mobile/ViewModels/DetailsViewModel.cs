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
    Stream _filePath, _stringBase64;

    public DetailsViewModel(IAssessmentService service)
    {
        DetailsIsVisible = true;
        EditIsVisible = false;
        _service = service;
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        StringBase64 = null;
        Assessment = null;
        Image.Source = null;

        if(FilePath is not null)
            FilePath.Dispose();

        var data = (Assessments)query["Data"];
        Assessment = data;
        Image.Source = ImageSource.FromUri(new Uri(Assessment.Image));

        DetailsIsVisible = true;
        EditIsVisible = false;
    }
    [RelayCommand]
    async void Edit()
    {
        DetailsIsVisible = !DetailsIsVisible;
        EditIsVisible = !EditIsVisible;
    }
    [RelayCommand]
    async void Save()
    {

        var result = await App.Current.MainPage.DisplayAlert("Editar", "Confimar alterações", "Sim", "Não");

        if (result)
        {
            Assessment.Image = await ConvertImageToBase64String(StringBase64);
            var assessment = await _service.PutAssessment(Assessment.Id, Assessment);
            WeakReferenceMessenger.Default.Send<Assessments>(assessment);

            Assessment = null;
            Image.Source = null;
            await StringBase64.DisposeAsync();
            await FilePath.DisposeAsync();

            await Shell.Current.GoToAsync("..");
        }
    }

    [RelayCommand]
    async void FileUpload()
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Selecione o imagem",
            FileTypes = FilePickerFileType.Images
        });

        if (result is null) return;
        FilePath = await result.OpenReadAsync();
        StringBase64 = await result.OpenReadAsync();


        Image.Source = ImageSource.FromStream(() => FilePath);
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
