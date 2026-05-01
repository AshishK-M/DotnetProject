using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;

namespace MyMvcApp.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products
                .FromSqlRaw("EXEC sp_GetProducts")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product?> GetProductById(int id)
        {
            var results = await _context.Products
                .FromSqlRaw("EXEC sp_GetProductById @Id",
                    new SqlParameter("@Id", id))
                .AsNoTracking()
                .ToListAsync();
            return results.FirstOrDefault();
        }

        public async Task AddProduct(Product product)
        {
            var featuresJson = JsonSerializer.Serialize(product.Features ?? new());

            var ids = await _context.Database
                .SqlQueryRaw<int>(
                    "EXEC sp_AddProduct @Name, @Price, @Description, @IsFeatured, @Features",
                    new SqlParameter("@Name", product.Name),
                    new SqlParameter("@Price", product.Price),
                    new SqlParameter("@Description", product.Description),
                    new SqlParameter("@IsFeatured", product.IsFeatured),
                    new SqlParameter("@Features", featuresJson))
                .ToListAsync();

            product.Id = ids.First();
        }

        public async Task UpdateProduct(Product product)
        {
            var featuresJson = JsonSerializer.Serialize(product.Features ?? new());

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_UpdateProduct @Id, @Name, @Price, @Description, @IsFeatured, @Features",
                new SqlParameter("@Id", product.Id),
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@Price", product.Price),
                new SqlParameter("@Description", product.Description),
                new SqlParameter("@IsFeatured", product.IsFeatured),
                new SqlParameter("@Features", featuresJson));
        }

        public async Task DeleteProduct(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_DeleteProduct @Id",
                new SqlParameter("@Id", id));
        }
    }
}
