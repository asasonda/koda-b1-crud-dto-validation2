using KodaStore.Api.DTOs;
using KodaStore.Api.Models;
using KodaStore.Api.Repositories;

namespace KodaStore.Api.Services;

public sealed class CustomerServices : ICustomerServices
{
    private readonly ICustomerRepository _repository;
    public CustomerServices(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var customers = await _repository.GetAllAsync(cancellationToken);
        return customers.Select(CustomerResponse.FromModel).ToArray();
    }

    public async Task<CustomerResponse?> GetByCodeAsync(string customerCode, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(customerCode) || customerCode.Trim().Length > 30)
        {
            return null;
        }
        var customers = await _repository.GetByCodeAsync(customerCode, cancellationToken);
        return customers is null ? null : CustomerResponse.FromModel(customers);
    }

    public async Task<CustomerResponse?> CreateAsync(CustomersRequest request, CancellationToken cancellationToken)
    {
        var customers = new Customers(request.CustomerCode, request.Name, request.Email, request.Phone, request.Address);
        var created = await _repository.AddCustomersAsync(customers, cancellationToken);
        return created ? CustomerResponse.FromModel(customers) : null;
    }

    public async Task<CustomerResponse?> UpdateAsync(string customerCode, UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customers = new Customers(customerCode, request.Name, request.Email, request.Phone, request.Address);
        var updated = await _repository.UpdateCustomersAsync(customers, cancellationToken);
        return updated ? CustomerResponse.FromModel(customers) : null;
    }

    public Task<bool> DeleteAsync(string customerCode, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(customerCode) || customerCode.Trim().Length > 30)
        {
            return Task.FromResult(false);
        }
        return _repository.DeleteCustomerAsync(customerCode, cancellationToken);
    }
}