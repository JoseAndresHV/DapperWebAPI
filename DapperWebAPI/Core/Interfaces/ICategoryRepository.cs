using DapperWebAPI.Core.Entities;

namespace DapperWebAPI.Core.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetByIdWithProducts(int id);
    }
}
