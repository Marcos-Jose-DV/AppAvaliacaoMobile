namespace Herpels.FileHelpers;

public static class ConverterImage
{
    public static async Task<string> ConvertImageToBase64String(Stream stream)
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
