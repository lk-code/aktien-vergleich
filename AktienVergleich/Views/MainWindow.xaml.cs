using AktienVergleich.Models;
using AktienVergleich.ViewModels;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;

namespace AktienVergleich.Views
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainViewModel ViewModel { get; set; } = null;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this.ViewModel = App.GetService<MainViewModel>();

            this.Loaded += ImportWindow_Loaded;
        }

        private async void ImportWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await this.ViewModel.InitializeAsync();
        }

        private void CompaniesCollectionComboBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string content = (e.OriginalSource as TextBox)!.Text;
            ((sender as ComboBox)!.DataContext as Share)!.CompanyLookupCommand.Execute(content);
        }
    }
}
