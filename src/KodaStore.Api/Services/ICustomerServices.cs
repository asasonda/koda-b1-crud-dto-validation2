using KodaStore.Api.DTOs;

namespace KodaStore.Api.Services;

public interface ICustomerServices
{
    Task<IReadOnlyList<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<CustomerResponse?> GetByCodeAsync(string customerCode, CancellationToken cancellationToken);
    Task<CustomerResponse?> CreateAsync(CustomersRequest request, CancellationToken cancellationToken);
    Task<CustomerResponse?> UpdateAsync(string customerCode, UpdateCustomerRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string customerCode, CancellationToken cancellationToken);

}