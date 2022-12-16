using DapperWebAPI.Core.Interfaces;

namespace DapperWebAPI.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(ICategoryRepository categoryRepository, IProductRepository productRepository, ISupplierRepository supplierRepository)
        {
            Categories = categoryRepository;
            Products = productRepository;
            Suppliers = supplierRepository;
        }

        public ICategoryRepository Categories { get; }
        public IProductRepository Products { get; }
        public ISupplierRepository Suppliers { get; }
    }
}
