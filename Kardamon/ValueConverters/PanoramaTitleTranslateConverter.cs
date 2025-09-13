using System.Collections;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Kardamon.ValueConverters;

namespace Kardamon;

public sealed class PanoramaTitleTranslateConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return System.Convert.ToDouble(value) * -0.40;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

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
        throw new NotImplementedException();
    }
}