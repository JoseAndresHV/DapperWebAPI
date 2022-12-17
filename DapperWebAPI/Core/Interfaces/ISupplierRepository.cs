using DapperWebAPI.Core.Entities;

namespace DapperWebAPI.Core.Interfaces
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        Task<IEnumerable<Supplier>> GetAllWithProducts();
    }
}
