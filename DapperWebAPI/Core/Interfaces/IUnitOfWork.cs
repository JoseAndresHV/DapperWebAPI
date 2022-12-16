namespace DapperWebAPI.Core.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        ISupplierRepository Suppliers { get; }
    }
}
