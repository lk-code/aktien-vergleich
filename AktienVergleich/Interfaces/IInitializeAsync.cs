using System.Threading.Tasks;

namespace AktienVergleich.Interfaces
{
    public interface IInitializeAsync
    {
        /// <summary>
        /// implements the initialize-method
        /// </summary>
        /// <returns></returns>
        Task InitializeAsync();
    }
}
