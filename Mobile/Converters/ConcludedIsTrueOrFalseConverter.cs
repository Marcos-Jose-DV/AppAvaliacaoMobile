using System.Globalization;

namespace Mobile.Converters;

public class ConcludedIsTrueOrFalseConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool result = (bool)value;

        if (result)
            return "Sim.";

        return "Não";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string result = (string)value;

        if (result is null) return "";

        if (result.Equals("Sim")) return "Sim.";

        return "Não";
    }
}
