// using KodaStore.Api.Data;
// using KodaStore.Api.Models;

// namespace KodaStore.Api.Repositories;

// public class InMemoryProductRepository : IProductRepository
// {
//     private readonly InMemoryProductCatalog _catalog;

//     public InMemoryProductRepository(InMemoryProductCatalog catalog)
//     {
//         _catalog = catalog;
//     }

//     public bool AddProduct(Products products)
//     {
//         return _catalog.AddProductList(products);

//     }

//     public bool DeleteProduct(string productCode)
//     {
//         return _catalog.Delete(productCode);
//     }

//     public IReadOnlyList<Products> GetAll()
//     {
//         return _catalog.GetAll();
//     }

//     public Products? GetByCode(string productCode)
//     {
//         return _catalog.GetByCode(productCode);
//     }

//     public bool UpdateProduct(Products products)
//     {
//         return _catalog.UpdateProductList(products);
//     }
// }