using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Helpers.FileHelper;
using Mobile.Herpels;
using Mobile.Services;
using Models.Models;

namespace Mobile.ViewModels;

public partial class DownloadViewModel : ObservableObject
{

    private readonly IFileSaver _fileSaver;
    private readonly AssessmentService _assessmentService;

    public DownloadViewModel(IFileSaver fileSaver, AssessmentService assessmentService)
    {
        _fileSaver = fileSaver;
        _assessmentService = assessmentService;
    }

    [RelayCommand]
    async Task Inport()
    {
        var result = await FileManupulation.FileUplod();

        List<Assessments> assessments = ReadFile.ReadExcel(result.FullPath);

        var ok = await _assessmentService.PostAssessments(assessments);

        Console.WriteLine(ok);
    }
    [RelayCommand]
    async Task SaveFile(string fileName)
    {
        var cancellationToken = new CancellationToken();
        try
        {
            var assessments = await _assessmentService.GetAssessments("");
            Stream stream = null;

            if (fileName.Equals("assessments.txt"))
            {
                stream = WriteFile.WriteTxt(assessments.Item2);
            }
            else if (fileName.Equals("assessments.pdf"))
            {
                stream = WriteFile.WritePDF(assessments.Item2);
            }
            else if (fileName.Equals("assessments.xlsx"))
            {
                stream = WriteFile.WriteExcel(assessments.Item2);
            }
            else
            {
                stream = WriteFile.WriteDoxc(assessments.Item2);
            }

            var fileLocationResult = await _fileSaver.SaveAsync(fileName, stream, cancellationToken);
            fileLocationResult.EnsureSuccess();
            await stream.DisposeAsync();
            await Toast.Make($"Arquivo salvo em: {fileLocationResult.FilePath}").Show(cancellationToken);
        }
        catch (Exception ex)
        {
            await Toast.Make($"O arquivo não foi salvo: {ex.Message}").Show(cancellationToken);
        }
    }

 

    [RelayCommand]
    private void Drag()
    {
      
    }
    [RelayCommand]
    private async void Drop()
    {
    }

    [RelayCommand]
    private async void Leave()
    {
    }
    [RelayCommand]
    private async void Completed()
    {
    }
}
