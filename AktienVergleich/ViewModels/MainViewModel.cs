using AktienVergleich.Interfaces;
using AktienVergleich.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AktienVergleich.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region properties

        private readonly SynchronizationContext _synchronizationContext = null;
        private readonly IShareService _shareService;
        private readonly ICurrencyConverterService _currencyConverterService;

        private string _loggingContent = string.Empty;
        public string LoggingContent
        {
            get { return _loggingContent; }
            set { SetProperty(ref _loggingContent, value); }
        }

        private ObservableCollection<Share> _aktienCollection = null;
        public ObservableCollection<Share> ShareCollection
        {
            get { return _aktienCollection; }
            set { SetProperty(ref _aktienCollection, value); }
        }

        private ObservableCollection<int> _intervalCollection = null;
        public ObservableCollection<int> IntervalCollection
        {
            get { return _intervalCollection; }
            set { SetProperty(ref _intervalCollection, value); }
        }

        private ICommand _addAktieCommand;
        public ICommand AddAktieCommand => _addAktieCommand ?? (_addAktieCommand = new RelayCommand(() =>
        {
            this.HandleAddAktie();
        }));

        #endregion

        #region constrcutors

        public MainViewModel()
        {
            this._synchronizationContext = SynchronizationContext.Current;

            this.ShareCollection = new ObservableCollection<Share>();
            this.IntervalCollection = new ObservableCollection<int>();
        }

        public MainViewModel(ILoggingService loggingService,
            IShareService shareService,
            ICurrencyConverterService currencyConverterService,
            IConfiguration configuration) : base(loggingService, configuration)
        {
            this._shareService = shareService ?? throw new ArgumentNullException(nameof(shareService));
            this._currencyConverterService = currencyConverterService ?? throw new ArgumentNullException(nameof(currencyConverterService));

            this._synchronizationContext = SynchronizationContext.Current;

            this.ShareCollection = new ObservableCollection<Share>();
            this.IntervalCollection = new ObservableCollection<int>();
        }

        ~MainViewModel()
        {
            this._loggingService.LogMessageReceived -= LoggingService_LogMessageReceived;
        }

        #endregion

        #region logic

        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
            this._loggingService.LogMessageReceived -= LoggingService_LogMessageReceived;
            this._loggingService.LogMessageReceived += LoggingService_LogMessageReceived;

            for (int i = 1; i <= 12; i++)
            {
                this.IntervalCollection.Add(i);
            }

            this.HandleAddAktie();
        }

        private void LoggingService_LogMessageReceived(object? sender, string loggingMessage)
        {
            this.LoggingContent += loggingMessage;
        }

        private void HandleAddAktie()
        {
            this.ShareCollection.Add(
                new Share(
                this._configuration,
                this._shareService,
                this._currencyConverterService));
        }

        #endregion
    }
}