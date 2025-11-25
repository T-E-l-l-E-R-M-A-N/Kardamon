using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace Kardamon;

public class DecodedImageSourceConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (!string.IsNullOrEmpty(value?.ToString()))
        {
            try
            {
                int decodeFactor = System.Convert.ToInt32(parameter);
                using var stream = new MemoryStream(new WebClient().DownloadData(value.ToString()));
                var imageSource =
                    Bitmap.DecodeToWidth(stream, decodeFactor, BitmapInterpolationMode.LowQuality);
                return imageSource;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return null!;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}