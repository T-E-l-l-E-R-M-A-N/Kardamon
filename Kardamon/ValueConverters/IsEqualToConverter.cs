using System.Globalization;

namespace Kardamon.ValueConverters;

public sealed class IsEqualToConverter : BaseMultiValueConverter
{
    public override object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        return values.FirstOrDefault() == values.LastOrDefault();
    }
}