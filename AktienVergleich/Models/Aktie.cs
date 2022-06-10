using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace AktienVergleich.Models
{
    public class Aktie : ObservableObject
    {
        #region Eingabe-Properties

        /// <summary>
        /// Der Name der Aktie
        /// </summary>
        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
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

                this.Calculate();
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

                this.Calculate();
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

                this.Calculate();
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

        public Aktie()
        {

        }

        #endregion

        #region logic

        /// <summary>
        /// 
        /// </summary>
        private void Calculate()
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

        #endregion
    }
}
