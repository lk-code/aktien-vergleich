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

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            // 1. initialize Dipendency Injection
            await AktienVergleich.Startup.Init();

            // 2. startup Logging
            App.ServiceProvider = AktienVergleich.Startup.ServiceProvider!;
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
