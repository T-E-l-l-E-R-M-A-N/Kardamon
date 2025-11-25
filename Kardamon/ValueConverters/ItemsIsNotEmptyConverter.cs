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
        return null!;
    }
}

public sealed class ItemSpacingConverter : BaseValueConverter
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null) return -(double)value;
        return 0;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null!;
    }
}

public sealed class DistanceCenterConverter : BaseValueConverter
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (value != null) 
                    return s == "neg" ? -(double)value : (double)value;
            }
        }

        return (double)value;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null!;
    }
}
