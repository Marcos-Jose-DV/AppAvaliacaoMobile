using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Herpels.FileHelpers;
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
    async Task Inport(CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Assessments> assessments = await ReadFile.ReadExcel();
            await _assessmentService.PostAssessments(assessments);
            await Toast.Make($"Arquivo foi carregado com sucesso.").Show(cancellationToken);
        }
        catch (Exception ex)
        {
            await Toast.Make($"Erro ao carregar arquivo: {ex.Message}").Show(cancellationToken);
        }
    }
    [RelayCommand]
    async Task SaveFile(string fileName)
    {
        var cancellationToken = new CancellationToken();
        try
        {
            var assessments = await _assessmentService.GetAssessments("");
            var stream = Helpers.FileHelper.SaveFile.Save(fileName, assessments.Item2);

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
}
