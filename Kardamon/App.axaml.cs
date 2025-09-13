using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Autofac;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Kardamon.Services;
using Kardamon.ViewModels;
using Kardamon.Views;

namespace Kardamon;

public partial class App : Application
{
    
    
    public override void Initialize()
    {
       //IoC.RegisterServices();

       var settings = IoC.Resolve<SettingsService>();
       if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
       {
           settings.ChangeSetting("IsMobile", "false");
       }
       else if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
       {
           settings.ChangeSetting("IsMobile", "false");
       }
       else
       {
           settings.ChangeSetting("IsMobile", "true");
       }

       IoC.Resolve<IPlayback>().Init();
       AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var vm = IoC.Resolve<MainViewModel>();
        vm.Init();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new Main
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}