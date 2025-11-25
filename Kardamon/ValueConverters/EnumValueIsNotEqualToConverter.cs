using System.Globalization;

namespace Kardamon.ValueConverters;

public sealed class EnumValueIsNotEqualToConverter : BaseMultiValueConverter
{
    public override object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    { 
        var status = values.FirstOrDefault().ToString() != values.LastOrDefault().ToString();
        return status;
    }
}