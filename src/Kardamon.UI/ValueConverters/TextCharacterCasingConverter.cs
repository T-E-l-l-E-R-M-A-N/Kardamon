using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Kardamon.Core;
using Kardamon.Core.ViewModels.Pages;
using Prism.Mvvm;

namespace Kardamon.UI.ValueConverters
{
    public abstract class ValueConverterBase : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
        public abstract object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture);
        public abstract object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture);
    }
    public abstract class MultiValueConverterBase : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
        public abstract object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture);
    }

    internal class ObjectConverter : MultiValueConverterBase
    {
        public override object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (parameter is string s)
            {
                switch (s.ToLower())
                {
                    case "isequal":
                        if (values[0] == values[1])
                            return true;
                        break;
                    case "isnotequal":
                        if (values[0] != values[1])
                            return true;
                        break;
                }
            }
            
            return false;
        }
    }
    internal class TextCharacterCasingConverter : ValueConverterBase
    {

        public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var text = value.ToString();

                return parameter switch
                {
                    "upper" => text?.ToUpper(),
                    "lower" => text?.ToLower(),
                    _ => text
                };
            }

            return value;
        }

        public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    internal class ApplicationMenuItemIconConverter : ValueConverterBase
    {
        public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is IPage p)
            {

                return p.Type switch
                {
                    PageType.Home => "/Assets/home.svg",
                    PageType.Library => "/Assets/library.svg",
                    PageType.Favorites => "/Assets/favorite.svg",
                    PageType.Downloads => "/Assets/download.svg",
                    PageType.Search => "/Assets/search.svg",
                    PageType.Settings => "/Assets/settings.svg",
                    
                };
            }

            return value;
        }

        public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
}