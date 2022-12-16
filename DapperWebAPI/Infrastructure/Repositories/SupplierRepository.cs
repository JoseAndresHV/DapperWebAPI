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
            var sql = "insert into Suppliers values (@SupplierID, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax, @HomePage)";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.ExecuteAsync(sql, entity);

            return result > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var sql = "delete from Suppliers where SupplierID = @SupplierID";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.ExecuteAsync(sql, new { SupplierID = id });

            return result > 0;
        }

        public async Task<IEnumerable<Supplier>> GetAll()
        {
            var sql = "select * from Suppliers";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.QueryAsync<Supplier>(sql);

            return result;
        }

        public async Task<Supplier?> GetById(int id)
        {
            var sql = "select * from Suppliers where SupplierID = @SupplierID";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.QuerySingleOrDefaultAsync<Supplier>(sql, new { SupplierID = id });

            return result;
        }

        public async Task<bool> Update(Supplier entity)
        {
            var sql = "update Suppliers set SupplierID = @SupplierID, CompanyName = @CompanyName, ContactName = @ContactName, ContactTitle = @ContactTitle, Address = @Address, City = @City, Region = @Region, PostalCode = @PostalCode, Country = @Country, Phone = @Phone, Fax = @Fax, HomePage = @HomePage where SupplierID = @SupplierID";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.ExecuteAsync(sql, entity);

            return result > 0;
        }
    }
}
