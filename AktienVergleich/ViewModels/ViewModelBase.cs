using AktienVergleich.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace AktienVergleich.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        #region Properties

        protected readonly ILoggingService _loggingService;
        protected readonly IConfiguration _configuration;

        #endregion

        #region Konstruktoren

        public ViewModelBase()
        {

        }

        public ViewModelBase(ILoggingService loggingService,
            IConfiguration configuration) : this()
        {
            this._loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            this.LoadDesignerInstance();
        }

        #endregion

        #region Worker

        protected virtual void LoadDesignerInstance()
        {

        }

        #endregion
    }
}
