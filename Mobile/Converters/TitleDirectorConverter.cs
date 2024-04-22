using System.Globalization;

namespace Mobile.Converters;

public class TitleDirectorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var category = value as string;

        return category switch
        {
            "Book" => "Livros relacionados ao autor",
            "Movie" => "Filmes relacionados ao diretor",
            "Série" => "Série relacionados ao diretor",
            _ => "Musicas relacionados a banda/cantor"

        };

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }


}
