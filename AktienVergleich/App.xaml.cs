using AktienVergleich.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace AktienVergleich
{
    public partial class App : Application
    {
        #region properties

        public static IServiceProvider? ServiceProvider { get; set; }

        #endregion

        #region constructor

        public App()
        {

        }

        #endregion

        #region logic

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. ui culture
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            // 2. initialize Dipendency Injection
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddAppConfig()
                .AddPlatform()
                .AddViewModel()
                ;

            App.ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// returns the requested service from di
        /// </summary>
        /// <typeparam name="T">The requested DI-Service</typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            return (T)App.ServiceProvider!.GetService(typeof(T))!;
        }

        #endregion
    }
}
