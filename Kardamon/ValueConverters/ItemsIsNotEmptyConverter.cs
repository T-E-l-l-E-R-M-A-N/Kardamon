using System.Globalization;
using Kardamon.ValueConverters;

namespace Kardamon;

public sealed class ItemsIsNotEmptyConverter : BaseValueConverter
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null)
        {
            var items = value as IEnumerable<object>;
            var count = items.Count();
            if (count == 0)
                return false;
        }

        if (value == null)
            return false;
        
        return true;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw null!;
    }
}