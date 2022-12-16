using Dapper;
using DapperWebAPI.Core.Entities;
using DapperWebAPI.Core.Interfaces;
using Microsoft.Data.SqlClient;

namespace DapperWebAPI.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<bool> Add(Category entity)
        {
            var sql = "insert into Categories values (@CategoryID, @CategoryName, @Description, @Picture)";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.ExecuteAsync(sql, entity);

            return result > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var sql = "delete from Categories where CategoryID = @CategoryID";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.ExecuteAsync(sql, new { CategoryID = id });

            return result > 0;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var sql = "select * from Categories";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.QueryAsync<Category>(sql);

            return result;
        }

        public async Task<Category?> GetById(int id)
        {
            var sql = "select * from Categories where CategoryID = @CategoryID";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.QuerySingleOrDefaultAsync<Category>(sql, new { CategoryID = id });

            return result;
        }

        public async Task<bool> Update(Category entity)
        {
            var sql = "update Categories set CategoryID = @CategoryID, CategoryName = @CategoryName, Description = @Description, Picture = @Picture where CategoryID = @CategoryID";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = await connection.ExecuteAsync(sql, entity);

            return result > 0;
        }
    }
}
