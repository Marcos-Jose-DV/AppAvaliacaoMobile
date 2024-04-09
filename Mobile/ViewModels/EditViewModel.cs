using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mobile.Models;
using Mobile.Services.Interfaces;
using System.IO;

namespace Mobile.ViewModels;

public partial class EditViewModel : ObservableObject, IQueryAttributable
{
    private readonly IAssessmentService _assessmentService;



    [ObservableProperty]
    Assessments _assessment;


    [ObservableProperty]
    Image _image = new();

    public EditViewModel(IAssessmentService assessmentService)
    {
        _assessmentService = assessmentService;
    }



    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var data = (Assessments)query["Data"];
        Assessment = data;
        Image.Source = ImageSource.FromUri(new Uri(Assessment.Image));
    }

    [RelayCommand]
    async void Edit()
    {

        var result = await App.Current.MainPage.DisplayAlert("Editar", "Confimar alterações", "Sim", "Não");

        if (result)
        {

            await _assessmentService.PutAssessment(Assessment.Id, Assessment);
            Assessment = null;

            WeakReferenceMessenger.Default.Send<string>(new string("Load"));
            await Shell.Current.GoToAsync("..");
        }
    }

    [RelayCommand]
    async void FileUpload()
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Selecione o image",
            FileTypes = FilePickerFileType.Images
        });

        if (result is null) return;

        var stream = await result.OpenReadAsync();
        var image = await result.OpenReadAsync();

        Image.Source = ImageSource.FromStream(() => stream);
        Assessment.Image = await ConvertImageToBase64String(image);
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
