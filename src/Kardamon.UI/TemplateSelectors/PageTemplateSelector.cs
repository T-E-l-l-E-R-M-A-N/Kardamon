using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Styling;
using Kardamon.Core.ViewModels.Pages;

namespace Kardamon.UI.TemplateSelectors
{
    public class PageTemplateSelector : IDataTemplate
    {
        public Control? Build(object? param)
        {
            var resources = App.Current.Resources;
            var name = param.GetType().Name.Replace("ViewModel", "");
            object template;
            resources.TryGetResource(name, ThemeVariant.Default, out template);
            if (template is DataTemplate dt)
                return dt.Build(param);

            return null;
        }

        public bool Match(object? data)
        {
            return data is IPage;
        }
    }
}