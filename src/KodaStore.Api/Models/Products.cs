namespace KodaStore.Api.Models;

public sealed class Products
{
    public string ProductCode {get; set;}
    public string Name {get; set;}
    public decimal Price {get; set;}
    public int Stock {get; set;}
    public string Category {get; set;}

    public Products(string productCode, string name, decimal price, int stock, string category)
    {
        if (string.IsNullOrWhiteSpace(productCode))
        {
            throw new ArgumentException("Product is required", nameof(productCode));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("name is required", nameof(name));
        }
        if (price < 0)
        {
            throw new ArgumentOutOfRangeException("Price not negative", nameof(price));
        }
        if (stock < 0)
        {
            throw new ArgumentOutOfRangeException("stock not negative", nameof(stock));
        }
        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException("category is required", nameof(category));
        }

        ProductCode = productCode;
        Name = name;
        Price = price;
        Stock = stock;
        Category = category.Trim();
    }
}