using Mobile.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Converters
{
    internal class TitleCardValueConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var assessment = value as Assessments;

            if (assessment is null) return "";

            return assessment.Category switch
            {
                "Book" => $"Paginas: {assessment.Duration}",
                "Série" => $"Temporadas: {assessment.Duration}",
                _ => $"Duração: {assessment.Duration}",
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }
    }
}
