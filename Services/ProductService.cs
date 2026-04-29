using MyMvcApp.Models;

namespace MyMvcApp.Services;

public class ProductService
{
    private readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Starter Plan", Price = 9.99m, Description = "Everything you need to get going." },
        new Product { Id = 2, Name = "Pro Plan", Price = 29.99m, Description = "For growing teams that need more power." },
        new Product { Id = 3, Name = "Enterprise Plan", Price = 99.99m, Description = "Advanced controls and dedicated support." }
    };

    public IEnumerable<Product> GetAllProducts() => _products;

    public Product? GetProductById(int id) => _products.FirstOrDefault(p => p.Id == id);
}
