using System.Threading.Tasks;

namespace AktienVergleich.Interfaces
{
    public interface ICurrencyConverterService
    {
        public Task<double> ConvertAsync(double value, string from, string to);
    }
}
