using System;
using Kardamon.Core.ViewModels.Pages;
using Kardamon.Core.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Kardamon.Core
{
    public static class IoC
    {
        private static IServiceProvider _provider;

        public static void Build()
        {
            var services = new ServiceCollection();

            services.AddScoped<ModelFactory>();
            services.AddScoped<PageFactory>();
            services.AddScoped<SelectionHost>();
            services.AddScoped<IPlayer, UniversalPlayerService>();

            services.AddScoped<MainWindowViewModel>();
            services.AddScoped<IPage, HomePageViewModel>();
            services.AddScoped<IPage, SearchPageViewModel>();

            _provider = services.BuildServiceProvider();
        }

        public static T Resolve<T>() => _provider.GetRequiredService<T>();
    }
}