using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Kardamon.Core;
using Kardamon.Core.ViewModels.Windows;
using Kardamon.UI.Windows;

namespace Kardamon.UI
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            IoC.Build();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var model = IoC.Resolve<MainWindowViewModel>();
            model.Init();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow() { DataContext = model };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }

}