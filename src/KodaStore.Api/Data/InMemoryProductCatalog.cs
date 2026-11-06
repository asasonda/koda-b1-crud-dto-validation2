using KodaStore.Api.Models;

namespace KodaStore.Api.Data;

public sealed class InMemoryProductCatalog
{
    private readonly object _sync = new();
    private readonly List<Products> _products = [];

    public IReadOnlyList<Products> GetAll()
    {
        lock (_sync)
        {
            return _products.ToArray();
        }
    }

    public bool AddProductList(Products products)
    {
        lock (_sync)
        {
            if(_products.Any(p => string.Equals(p.ProductCode,products.ProductCode, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
        }

        _products.Add(products);
        return true;
    }

    public Products? GetByCode(string productCode)
    {
        lock (_sync)
        {
            return _products.FirstOrDefault(p => string.Equals(p.ProductCode, productCode.Trim(),StringComparison.OrdinalIgnoreCase));
        }
    }

    public bool UpdateProductList(Products products)
    {
        lock (_sync)
        {
            var find = _products.FindIndex(p => string.Equals(p.ProductCode, products.ProductCode, StringComparison.OrdinalIgnoreCase));

            if(find < 0)
            {
                return false;
            }

            _products[find] = products;

            return true;
        }
    }

    public bool Delete(string productCode)
    {
        lock (_sync)
        {
            var find = _products.FindIndex(p => string.Equals(p.ProductCode, productCode.Trim(), StringComparison.OrdinalIgnoreCase));

            if(find < 0)
            {
                return false;
            }

            _products.RemoveAt(find);
            return true;
        }
    }

}