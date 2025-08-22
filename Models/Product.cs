namespace ProductApi.Models;
public class Product
{
    public int Id { get; set; }
    public string ProductCode { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public decimal Price { get; set; }
}
