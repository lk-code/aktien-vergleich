using AktienVergleich.Interfaces;
using AktienVergleich.Services.CurrencyConverter;
using AktienVergleich.Services.Logging;
using AktienVergleich.Services.Share;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;

namespace AktienVergleich.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAppConfig(this IServiceCollection services)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            string appSettingFileResourceName = $"{typeof(App).Namespace}.appsettings.json";
            Stream stream = assembly.GetManifestResourceStream(appSettingFileResourceName);
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            services.AddSingleton<IConfiguration>(config);

            return services;
        }
        
        public static IServiceCollection AddPlatform(this IServiceCollection services)
        {
            services.AddSingleton<ILoggingService, LoggingService>();
            services.AddSingleton<IShareService, ShareService>();
            services.AddSingleton<ICurrencyConverterService, CurrencyConverterService>();

            return services;
        }

        public static IServiceCollection AddViewModel(this IServiceCollection services)
        {
            services.AddSingleton<ViewModels.MainViewModel>();

            return services;
        }
    }
}
