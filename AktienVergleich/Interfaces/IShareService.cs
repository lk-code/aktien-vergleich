using AktienVergleich.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AktienVergleich.Interfaces
{
    public interface IShareService
    {
        Task<List<Company>> GetCompaniesAsync(string name = null);
        Task<Company> GetCompanyDetailsAsync(Company company);
        Task<List<Dividend>> GetDividendsAsync(string symbol);
    }
}
