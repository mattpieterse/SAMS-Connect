using System.Globalization;
using System.Windows.Data;

namespace Connect.UI.Models.Converters;

public sealed class TypeToBooleanConverter : IValueConverter
{
    public Type? TargetType { get; set; }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value == null || TargetType == null) return false;
        return TargetType.IsInstanceOfType(value);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
