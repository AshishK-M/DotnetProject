namespace MyMvcApp.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public List<string> Features { get; set; } = new();
}
