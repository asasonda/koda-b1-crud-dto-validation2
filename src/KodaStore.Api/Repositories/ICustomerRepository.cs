using KodaStore.Api.DTOs;
using KodaStore.Api.Models;

namespace KodaStore.Api.Repositories;

public interface ICustomerRepository
{
    Task<IReadOnlyList<Customers>> GetAllAsync(CancellationToken cancellationToken);
    Task<Customers?> GetByCodeAsync(string customerCode, CancellationToken cancellationToken);
    Task<bool> AddCustomersAsync(Customers customers, CancellationToken cancellationToken);
    Task<bool> UpdateCustomersAsync(Customers customers, CancellationToken cancellationToken);
    Task<bool> DeleteCustomerAsync(string customerCode, CancellationToken cancellationToken);
}