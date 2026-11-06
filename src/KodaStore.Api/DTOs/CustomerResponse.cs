using KodaStore.Api.Models;

namespace KodaStore.Api.DTOs;

public sealed record CustomerResponse(string CustomerCode, string Name, string Email, string Phone, string Address)
{
    public static CustomerResponse FromModel(Customers customers) => new(customers.CustomerCode, customers.Name, customers.Email, customers.Phone, customers.Address);
}