using AktienVergleich.Interfaces;
using AktienVergleich.Services.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AktienVergleich.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPlatform(this IServiceCollection services)
        {
            services.AddSingleton<ILoggingService, LoggingService>();

            return services;
        }

        public static IServiceCollection AddViewModel(this IServiceCollection services)
        {
            services.AddSingleton<ViewModels.MainViewModel>();

            return services;
        }
    }
}
