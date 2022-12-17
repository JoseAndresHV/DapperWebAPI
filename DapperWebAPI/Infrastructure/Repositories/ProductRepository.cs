using Dapper;
using DapperWebAPI.Core.Entities;
using DapperWebAPI.Core.Interfaces;
using Microsoft.Data.SqlClient;

namespace DapperWebAPI.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<bool> Add(Product entity)
        {
            var sql = @"insert into Products (ProductName, SupplierID, CategoryID, QuantityPerUnit, 
                        UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued)
                        values (@ProductName, @SupplierID, @CategoryID, @QuantityPerUnit, 
                        @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel, @Discontinued)";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.ExecuteAsync(sql, entity);

            return result > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var sql = @"delete from Products 
                        where ProductID = @ProductID";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.ExecuteAsync(sql, new { ProductID = id });

            return result > 0;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var sql = @"select * from Products";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.QueryAsync<Product>(sql);

            return result;
        }

        public async Task<IEnumerable<Product>> GetAllDetailed()
        {
            var sql = @"select p.*, s.*, c.CategoryID, c.CategoryName from Products p
                        left join Suppliers s
                        on p.SupplierID = s.SupplierID
                        left join Categories c
                        on p.CategoryID = c.CategoryID";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.QueryAsync<Product, Supplier, Category, Product>(sql,
                (product, supplier, category) =>
                {
                    product.Supplier = supplier;
                    product.Category = category;
                    return product;
                }, splitOn: "SupplierId,CategoryId");

            return result;
        }

        public async Task<Product?> GetById(int id)
        {
            var sql = @"select * from Products 
                        where ProductID = @ProductID";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.QuerySingleOrDefaultAsync<Product>(sql, new { ProductID = id });

            return result;
        }

        public async Task<Product?> GetByIdDetailed(int id)
        {
            var sql = @"select p.*, s.*, c.CategoryID, c.CategoryName from Products p
                        left join Suppliers s
                        on p.SupplierID = s.SupplierID
                        left join Categories c
                        on p.CategoryID = c.CategoryID
                        where ProductID = @ProductID";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.QueryAsync<Product, Supplier, Category, Product>(
                sql,
                (product, supplier, category) =>
                {
                    product.Supplier = supplier;
                    product.Category = category;
                    return product;
                },
                param: new { ProductID = id },
                splitOn: "SupplierId,CategoryId");

            return result.FirstOrDefault();
        }

        public async Task<bool> Update(Product entity)
        {
            var sql = @"update Products set ProductName = @ProductName, 
                        SupplierID = @SupplierID, CategoryID = @CategoryID, 
                        QuantityPerUnit = @QuantityPerUnit, UnitPrice = @UnitPrice, 
                        UnitsInStock = @UnitsInStock, UnitsOnOrder = @UnitsOnOrder, 
                        ReorderLevel = @ReorderLevel, Discontinued = @Discontinued 
                        where ProductID = @ProductID";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.ExecuteAsync(sql, entity);

            return result > 0;
        }
    }
}
