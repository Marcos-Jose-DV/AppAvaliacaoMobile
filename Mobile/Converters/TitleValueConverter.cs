using Mobile.Models;
using System.Globalization;

namespace Mobile.Converters;

internal class TitleValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var category = value as string;

        if (category is null) return "";

        return category switch
        {
            "Book" => $"paginas",
            "Série" => $"temporadas",
            _ => $"min",
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();

    }
}
