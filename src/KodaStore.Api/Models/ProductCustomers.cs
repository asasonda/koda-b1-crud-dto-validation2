
namespace KodaStore.Api.Models;

public sealed class ProductCustomer(
    string productCode,
    string name,
    decimal price,
    int stock,
    string category,
    string customerName,
    string customerEmail
)
{
    public string ProductCode { get; set; } = productCode;
    public string Name { get; set; } = name;
    public decimal Price { get; set; } = price;
    public int Stock { get; set; } = stock;
    public string Category { get; set; } = category;
    public string CustomerName { get; set; } = customerName;
    public string CustomerEmail { get; set; } = customerEmail;
}
