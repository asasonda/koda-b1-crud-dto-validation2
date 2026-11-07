using KodaStore.Api.DTOs;
using KodaStore.Api.Models;

namespace KodaStore.Api.Repositories;

public interface IProductRepository
{
    Task<IReadOnlyList<Products>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<ProductCustomerResponse>> GetAllWithCustomerAsync(CancellationToken cancellationToken);
    Task<Products?> GetByCodeAsync(string productCode, CancellationToken cancellationToken);
    Task<bool> AddProductAsync(Products products, CancellationToken cancellationToken);
    Task<bool> UpdateProductAsync(Products products, CancellationToken cancellationToken);
    Task<bool> DeleteProductAsync(string productCode, CancellationToken cancellationToken);
}