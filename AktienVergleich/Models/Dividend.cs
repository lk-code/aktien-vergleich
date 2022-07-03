using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace AktienVergleich.Models
{
    public class Dividend : ObservableObject
    {
        /// <summary>
        /// Die Höhe der Dividende
        /// </summary>
        private double _sum = 0;
        public double Sum
        {
            get { return _sum; }
            set { SetProperty(ref _sum, value); }
        }
    }
}
