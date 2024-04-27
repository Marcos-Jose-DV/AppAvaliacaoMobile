namespace Herpels.FileHelpers;
public static class PickerFile
{
    public static async Task<FileResult> PickerFileAsync()
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

        return result is null ? throw new Exception("Nenhum arquivo foi selecionado.") : result;
    }
}
