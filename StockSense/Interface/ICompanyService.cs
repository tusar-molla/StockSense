using StockSense.Models;

namespace StockSense.Interface
{
    public interface ICompanyService
    {
        Task<List<Company>> GetAllAsync();
        Task<bool> CreateCompanyAsync(Company company);
        Task<bool> UpdateCompanyAsync(Company company);
    }
}
