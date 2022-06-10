using AktienVergleich.Interfaces;
using AktienVergleich.Models;
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

        private string _loggingContent = string.Empty;
        public string LoggingContent
        {
            get { return _loggingContent; }
            set { SetProperty(ref _loggingContent, value); }
        }

        private ObservableCollection<Aktie> _aktienCollection = null;
        public ObservableCollection<Aktie> AktienCollection
        {
            get { return _aktienCollection; }
            set { SetProperty(ref _aktienCollection, value); }
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

            this.AktienCollection = new ObservableCollection<Aktie>();
        }

        public MainViewModel(ILoggingService loggingService) : base(loggingService)
        {
            this._synchronizationContext = SynchronizationContext.Current;

            this.AktienCollection = new ObservableCollection<Aktie>();
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

            this.HandleAddAktie();
        }

        private void LoggingService_LogMessageReceived(object? sender, string loggingMessage)
        {
            this.LoggingContent += loggingMessage;
        }

        private void HandleAddAktie()
        {
            this.AktienCollection.Add(new Aktie());
        }

        #endregion
    }
}