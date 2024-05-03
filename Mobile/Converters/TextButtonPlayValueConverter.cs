using System.Globalization;

namespace Mobile.Converters;

public class TextButtonPlayValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      var text = (string)value;

        if (text == null || text != "Book") return "Assistir agora";

        return "Ler agora";

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
