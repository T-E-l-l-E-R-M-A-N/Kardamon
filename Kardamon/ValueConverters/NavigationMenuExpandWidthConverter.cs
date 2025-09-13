using System.Globalization;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Kardamon.Services;

namespace Kardamon.ValueConverters;

public sealed class NavigationMenuExpandWidthConverter : BaseValueConverter
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var c = value as Control;
        var main = c.GetVisualRoot();
        //var settings = IoC.Resolve<SettingsService>();
        
        if(main is Control window)
        {
            if (window.Bounds.Width < 400)
                return window.Bounds.Width;
        }
        
        return 210;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}