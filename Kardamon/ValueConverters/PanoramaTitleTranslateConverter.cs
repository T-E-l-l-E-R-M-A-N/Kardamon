using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
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

    public class AsyncTask : INotifyPropertyChanged
    {
        public AsyncTask(Func<object> valueFunc)
        {
            AsyncValue = "loading async value"; //temp value for demo
            LoadValue(valueFunc);
        }

        private async Task LoadValue(Func<object> valueFunc)
        {
            AsyncValue = await Task<object>.Run(() => { return valueFunc(); });
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("AsyncValue"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public object AsyncValue { get; set; }
    }
}