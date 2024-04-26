namespace Mobile.Herpels;

public static class FileManupulation
{
    public static async Task<FileResult> FileUplod()
    {

        var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            {DevicePlatform.WinUI, new[] {".pdf", ".xlsx",".txt", "docx"} }
        });

        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Selecione o arquivo",
            FileTypes = customFileType

        });


        return result;
    }
}
