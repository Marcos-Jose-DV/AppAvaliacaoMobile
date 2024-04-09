using System.Globalization;

namespace Mobile.Converters;

internal class AuthorValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var category = value as string;

        if (category is null) return category;

        return category switch
        {
            "Book" => $"Autor:",
            "Music" => $"Banda:",
            _ => $"Diretor:",

        }; 
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var category = value as string;

        if (category is null) return category;

        return category switch
        {
            "Book" => $"Autor:",
            "Music" => $"Banda:",
            _ => $"Diretor:",

        };
    }
}
