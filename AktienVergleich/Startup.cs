using AktienVergleich.Extensions;
using AktienVergleich.Interfaces;
using LKCode.Helper.Extensions;
using LKCode.Helper.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AktienVergleich
{
    public static class Startup
    {
        public static IServiceProvider? ServiceProvider { get; set; }

        public static async Task<IServiceProvider> Init()
        {
            List<Type> serviceTypes = new List<Type>();

            IServiceCollection serviceCollection = new ServiceCollection()

            .AddLKCodeConfig()
            .AddPlatform()
            .AddViewModel()
            ;

            foreach (ServiceDescriptor service in serviceCollection)
            {
                serviceTypes.Add(service.ServiceType);
            }

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(Startup)).Assembly;
            string ressourceNamespace = typeof(Startup).Namespace!;
            string configFileName = "appsettings.json";
            string appSettingFileResourceName = $"{ressourceNamespace}.{configFileName}";
            Stream stream = assembly.GetManifestResourceStream(appSettingFileResourceName)!;
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();

                IConfigService configService = serviceProvider.GetService<IConfigService>()!;
                configService.Initialize(JObject.Parse(json));
            }

            foreach (Type serviceType in serviceTypes)
            {
                object service = serviceProvider.GetService(serviceType);

                if (service is IInitializeAsync)
                {
                    await ((IInitializeAsync)service).InitializeAsync();
                }

                if (service is IInitialize)
                {
                    ((IInitialize)service).Initialize();
                }
            }

            ServiceProvider = serviceProvider;

            return serviceProvider;
        }
    }
}
