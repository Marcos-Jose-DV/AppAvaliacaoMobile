using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Mobile.Constans;
using Models.Models;
using System.Net;
using System.Web;

namespace Mobile.ViewModels;

public partial class PlayViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    MediaElement _video = new();

    [ObservableProperty]
    Assessments _assessment;

    [ObservableProperty]
    string _isBook;



    public PlayViewModel()
    {
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        IsBook = null;
        Video.Source = null;
        var teste = FileSystem.Current.AppDataDirectory;
        Assessments assessment = (Assessments)query["Data"];
        if (assessment.Category != "Book")
        {
            string fileName = Load(assessment, Configurations.ServePathMovie);

           Video.Source = MediaSource.FromFile(Configurations.ServePathMovie + "//" + fileName);
        }
        else
        {
            string fileName = Load(assessment, Configurations.ServePathBook);
            IsBook = "file:///" + Configurations.ServePathBook + "//" + WebUtility.UrlEncode(fileName);
        }

    }

    private string Load(Assessments assessment, string path)
    {
        string decodedName = HttpUtility.UrlDecode(assessment.Name);


        string[] fileEntries = Directory.GetFiles(path);
        foreach (string pathFile in fileEntries)
        {
            string fileName = Path.GetFileNameWithoutExtension(pathFile);
            if (fileName.Contains(decodedName, StringComparison.OrdinalIgnoreCase))
            {
                return fileName;
            }
        }
        return null;
    }

    [RelayCommand]
    async Task Load()
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Selecione o imagem",
            FileTypes = FilePickerFileType.Videos
        });

        if (result is null) return;
    }
}
