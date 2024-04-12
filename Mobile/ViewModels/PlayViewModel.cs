using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mobile.Models;
using System.Web;

namespace Mobile.ViewModels;

public partial class PlayViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    MediaElement _video = new();

    [ObservableProperty]
    Assessments _assessment;

    public PlayViewModel()
    {
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        string name = (string)query["Data"];
        string decodedName = HttpUtility.UrlDecode(name);


        string[] fileEntries = Directory.GetFiles("D:\\00_Servidor\\Movies");
        foreach (string fileName in fileEntries)
        {
            string file = Path.GetFileNameWithoutExtension(fileName);
            if (file.ToLower().Equals(decodedName.ToLower()))
            {
                Video.Source = MediaSource.FromFile(fileName);
                break;
            }
        }
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
