using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace AktienVergleich.Models
{
    public class Company : ObservableObject
    {
        /// <summary>
        /// Der Name des Unternehmes
        /// </summary>
        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        /// <summary>
        /// Der Name des Unternehmes
        /// </summary>
        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { SetProperty(ref _symbol, value); }
        }
        /// <summary>
        /// Das Logo des Unternehmes
        /// </summary>
        private string _logo = string.Empty;
        public string Logo
        {
            get { return _logo; }
            set { SetProperty(ref _logo, value); }
        }
    }
}
