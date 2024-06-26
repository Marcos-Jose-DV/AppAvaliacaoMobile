﻿
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Herpels.FileHelpers;
using Mobile.Services;
using Models.Models;

namespace Mobile.ViewModels;

public partial class AddCardViewModel : ObservableObject
{
    private readonly AssessmentService _service;

    [ObservableProperty]
    Assessments _assessment;

    [ObservableProperty]
    string[] _categories;

    [ObservableProperty]
    Image _image = new();

    [ObservableProperty]
    Stream _stringBase64;

    public AddCardViewModel(Services.AssessmentService service)
    {
        _service = service;
        CleanData();

        Categories = ["Book", "Série", "Movie", "Music"];
    }

    [RelayCommand]
    async Task Save()
    {
        var result = await App.Current.MainPage.DisplayAlert("Avaliação", "Criar nova avalição", "Sim", "Não");

        if (result)
        {
            if (Image.Source is not null)
            {
                Assessment.Image = await ConverterImage.ConvertImageToBase64String(StringBase64);
            }

            await _service.PostAssessment(Assessment);
            StringBase64.Dispose();
            await Shell.Current.GoToAsync("..");
        }
    }
    [RelayCommand]
    async Task Back()
    {
        await Shell.Current.GoToAsync("..");
    }
    [RelayCommand]
    void Clean()
    {
        CleanData();
    }
    public void CleanData()
    {
        Image.Source = null;
        Assessment = new();
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

            if (result is null) return;

            StringBase64 = await result.OpenReadAsync();
            Image.Source = ImageSource.FromFile(result.FullPath);
        }
        catch (Exception ex)
        {
            await Toast.Make($"Ocorreu um erro ao selecionado o arquivo.").Show(cancellationToken);
        }
    }
}
