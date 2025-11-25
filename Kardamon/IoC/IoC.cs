using Autofac;
using Kardamon.Factory;
using Kardamon.Services;
using Kardamon.ViewModels;

namespace Kardamon;

public static class IoC
{
    private static IContainer _container;

    public static void RegisterServices(ContainerBuilder? services = null!)
    {
        services ??= new ContainerBuilder();

        services.RegisterType<PageFactory>().SingleInstance();
        services.RegisterType<MainViewModel>().SingleInstance();
        services.RegisterType<NavigationMenuViewModel>().SingleInstance();
        services.RegisterType<CommandBarViewModel>().SingleInstance();
        services.RegisterType<MiniPlayerViewModel>().SingleInstance();
        services.RegisterType<ToastNotificationViewModel>().SingleInstance();
        
        services.RegisterType<ExplorePageViewModel>().As<IPage>().SingleInstance();
        services.RegisterType<NowPlayingPageViewModel>().As<IPage>().SingleInstance();
        services.RegisterType<FavoritesPageViewModel>().As<IPage>().SingleInstance();
        services.RegisterType<SearchPageViewModel>().As<IPage>().SingleInstance();
        
        services.RegisterType<NavigationService>().SingleInstance();
        services.RegisterType<LibraryService>().SingleInstance();
        services.RegisterType<WebSearchService>().SingleInstance();
        //services.RegisterType<PlaybackService>().SingleInstance();
        services.RegisterType<SettingsService>().SingleInstance();
        services.RegisterType<NotificationService>().SingleInstance();
        services.RegisterType<DownloadService>().SingleInstance();
        
        _container = services.Build();
    }
    
    public static T Resolve<T>() => _container.Resolve<T>();
}