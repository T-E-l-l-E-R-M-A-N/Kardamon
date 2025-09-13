using System.Globalization;
using Kardamon.ViewModels;
using Kardamon.Views;

namespace Kardamon.ValueConverters;

public sealed class NavigationMenuIconConverter : BaseValueConverter
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return "";
        
        return (value as IPage).Type switch
        {
            PageType.Index => "mdi heart",
            PageType.MyMusic => "mdi music",
            PageType.Settings => "mdi cog-outline",
            PageType.Search => "mdi magnify",
            PageType.About => "mdi information-outline",
            PageType.Web => "mdi web",
            PageType.NowPlaying => "mdi equalizer-outline",
            _ => "mdi view-grid-outline"
        };
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}