namespace KodaStore.Api.DTOs;

public record UpdateCustomerRequest
(
    string Name,
    string Email,
    string Phone,
    string Address
);