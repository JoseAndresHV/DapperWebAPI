using Dapper;
using DapperWebAPI.Core.Entities;
using DapperWebAPI.Core.Interfaces;
using Microsoft.Data.SqlClient;

namespace DapperWebAPI.Infrastructure.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SupplierRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<bool> Add(Supplier entity)
        {
            var sql = @"insert into Suppliers (CompanyName, ContactName, ContactTitle, 
                        Address, City, Region, PostalCode, Country, Phone, Fax, HomePage)
                        values (@CompanyName, @ContactName, @ContactTitle, 
                        @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax, @HomePage)";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.ExecuteAsync(sql, entity);

            return result > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var sql = @"delete from Suppliers 
                        where SupplierID = @SupplierID";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.ExecuteAsync(sql, new { SupplierID = id });

            return result > 0;
        }

        public async Task<IEnumerable<Supplier>> GetAll()
        {
            var sql = @"select * from Suppliers";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.QueryAsync<Supplier>(sql);

            return result;
        }

        public async Task<IEnumerable<Supplier>> GetAllWithProducts()
        {
            var sql = @"select *
                        from Suppliers s
                        left join Products p
                        on s.SupplierID = p.SupplierID";
            using var connection = new SqlConnection(_connectionString);
            var dictionary = new Dictionary<int, Supplier>();
            connection.Open();
            var result = await connection.QueryAsync<Supplier, Product, Supplier>(
                sql,
                (supplier, product) =>
                {
                    if (!dictionary.TryGetValue(supplier.SupplierID, out Supplier? entry))
                    {
                        entry = supplier;
                        entry.Products ??= new List<Product>();
                        dictionary.Add(supplier.SupplierID, entry);
                    }

                    if (product is not null)
                    {
                        entry.Products!.Add(product);
                    }
                    return entry;
                },
                splitOn: "ProductID");

            return result.Distinct();
        }

        public async Task<Supplier?> GetById(int id)
        {
            var sql = @"select * from Suppliers 
                        where SupplierID = @SupplierID";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.QuerySingleOrDefaultAsync<Supplier>(sql, new { SupplierID = id });

            return result;
        }

        public async Task<bool> Update(Supplier entity)
        {
            var sql = @"update Suppliers set CompanyName = @CompanyName, 
                        ContactName = @ContactName, ContactTitle = @ContactTitle, 
                        Address = @Address, City = @City, Region = @Region, 
                        PostalCode = @PostalCode, Country = @Country, 
                        Phone = @Phone, Fax = @Fax, HomePage = @HomePage 
                        where SupplierID = @SupplierID";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.ExecuteAsync(sql, entity);

            return result > 0;
        }
    }
}
