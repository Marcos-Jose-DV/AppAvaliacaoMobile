using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Storage;
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

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        IsBook = null;
        Video.Source = null;

        Assessments assessment = (Assessments)query["Data"];
        if (assessment.Category != "Book")
        {
            string pathFile = Load(assessment.Name, Configurations.ServePathMovie);
            Video.Source = MediaSource.FromFile(pathFile);
        }
        else
        {
            string pathFile = Load(assessment.Id.ToString(), Configurations.ServePathBook);
            IsBook = "file:///" + pathFile; ;
        }
    }

    private string Load(string filter, string path)
    {
        string decodedName = HttpUtility.UrlDecode(filter);
        string[] fileEntries = Directory.GetFiles(path);
        foreach (string pathFile in fileEntries)
        {
            string fileName = Path.GetFileNameWithoutExtension(pathFile);
            if (fileName.Contains(decodedName, StringComparison.OrdinalIgnoreCase))
            {
                return pathFile;
            }
        }
        return null;
    }
}
