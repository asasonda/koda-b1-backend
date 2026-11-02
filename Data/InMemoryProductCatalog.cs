using KodaStore.Api.Models;

namespace KodaStore.Api.Data;

public sealed class InMemoryProductCatalog
{
    private readonly List<Products> _products = [];

    public IReadOnlyList<Products> GetAll()
    {
        return _products.ToArray();
    }

    public Products? GetByCode(string productCode)
    {
        return _products.FirstOrDefault(product => string.Equals(product.ProductCode, productCode.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public void AddProduct(Products products)
    {
        _products.Add(products);
    }

    public void UpdateProduct(Products products)
    {
        
    }
}