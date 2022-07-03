using AktienVergleich.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AktienVergleich.Models
{
    public class Share : ObservableObject
    {
        #region Properties

        private readonly IConfiguration _configuration;
        private readonly IShareService _shareService;
        private readonly ICurrencyConverterService _currencyConverterService;

        private readonly bool _onlineShareSearchEnabled = false;

        private ObservableCollection<Company> _companiesCollection = null;
        public ObservableCollection<Company> CompaniesCollection
        {
            get { return _companiesCollection; }
            set { SetProperty(ref _companiesCollection, value); }
        }
        private ICommand _companyLookupCommand;
        public ICommand CompanyLookupCommand => _companyLookupCommand ?? (_companyLookupCommand = new RelayCommand<string>((eventArgs) =>
        {
            _ = this.LookUpForCompany(eventArgs);
        }));

        #endregion

        #region Eingabe-Properties

        /// <summary>
        /// Das Unternehmen
        /// </summary>
        private Company _company = null;
        public Company Company
        {
            get { return _company; }
            set { SetProperty(ref _company, value); }
        }
        /// <summary>
        /// Content für einen Fehler, etc.
        /// </summary>
        private string _error = string.Empty;
        public string Error
        {
            get { return _error; }
            set { SetProperty(ref _error, value); }
        }
        /// <summary>
        /// Der Interval (in Anzahl der Monate) der Dividenden-Ausschüttung
        /// 
        /// 1 = Monatlich
        /// 3 = Quartalweise
        /// 6 = Halbjährlich
        /// 12 = Jährlich
        /// </summary>
        private int _interval = 0;
        public int Interval
        {
            get { return _interval; }
            set
            {
                SetProperty(ref _interval, value);

                _ = this.CalculateAsync();
            }
        }
        /// <summary>
        /// Der Preis einer einzelnen Aktie
        /// </summary>
        private double _price = 0.00;
        public double Price
        {
            get { return _price; }
            set
            {
                SetProperty(ref _price, value);

                _ = this.CalculateAsync();
            }
        }
        /// <summary>
        /// Die Höhe der Dividende
        /// </summary>
        private double _dividendSum = 0.00;
        public double DividendSum
        {
            get { return _dividendSum; }
            set
            {
                SetProperty(ref _dividendSum, value);

                _ = this.CalculateAsync();
            }
        }

        #endregion

        #region Ausgabe-Properties

        /// <summary>
        /// Gibt die berechnete Dividende pro Monat an (zum Vergleich)
        /// </summary>
        private double _dividendPerMonthSum = 0.00;
        public double DividendPerMonthSum
        {
            get { return _dividendPerMonthSum; }
            set { SetProperty(ref _dividendPerMonthSum, value); }
        }

        /// <summary>
        /// Gibt die berechnete Dividende pro 100€ Aktien (zum Vergleich)
        /// </summary>
        private double _dividendPerSamePrice = 0.00;

        public double DividendPerSamePrice
        {
            get { return _dividendPerSamePrice; }
            set { SetProperty(ref _dividendPerSamePrice, value); }
        }

        #endregion

        #region constructors

        public Share()
        {
            this.CompaniesCollection = new ObservableCollection<Company>();
        }

        public Share(IConfiguration configuration,
            IShareService shareService,
            ICurrencyConverterService currencyConverterService) : this()
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this._shareService = shareService ?? throw new ArgumentNullException(nameof(shareService));
            this._currencyConverterService = currencyConverterService ?? throw new ArgumentNullException(nameof(currencyConverterService));

            this._onlineShareSearchEnabled = this._configuration.GetValue<bool>("Features:OnlineShareSearch", false);
        }

        #endregion

        #region logic

        /// <summary>
        /// 
        /// </summary>
        private async Task CalculateAsync()
        {
            this.Error = string.Empty;

            if (this.Interval < 1
                || this.Interval > 12)
            {
                this.Error = "Interval ist ungültig (1-12).";

                return;
            }

            if (this.Price <= 0)
            {
                this.Error = "Preis muss höher als 0 sein.";

                return;
            }

            if (this.DividendSum <= 0)
            {
                this.Error = "Die Dividende muss höher als 0 sein.";

                return;
            }

            try
            {
                int i = 0;

                double part = (this.DividendSum / (double)this.Interval);

                double result = Math.Round(part, 2);
                this.DividendPerMonthSum = result;

                double aktienPart = ((double)100 / this.Price);
                double samePriceDividend = Math.Round(aktienPart * part, 2);
                this.DividendPerSamePrice = samePriceDividend;
            }
            catch (Exception exception)
            {
                this.Error = exception.Message;
            }
        }

        private async Task LookUpForCompany(string name)
        {
            if (this._onlineShareSearchEnabled != true)
            {
                return;
            }

            this.CompaniesCollection.Clear();

            if (string.IsNullOrEmpty(name)
                || name.Length < 3)
            {
                return;
            }

            List<Company> companies = await this._shareService.GetCompaniesAsync(name);
            foreach (Company company in companies)
            {
                this.CompaniesCollection.Add(company);
            }
        }

        #endregion
    }
}
