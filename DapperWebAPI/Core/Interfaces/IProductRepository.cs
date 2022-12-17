using DapperWebAPI.Core.Entities;

namespace DapperWebAPI.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllDetailed();
        Task<Product?> GetByIdDetailed(int id);
    }
}
