using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Kardamon.ValueConverters;

public abstract class BaseValueConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public abstract object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture);
    public abstract object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture);
}