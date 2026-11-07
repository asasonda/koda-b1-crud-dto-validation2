namespace KodaStore.Api.DTOs;

public record ProductCustomerResponse(
    string ProductCode,
    string Name,
    decimal Price,
    int Stock,
    string Category,
    string CustomerName,
    string CustomerEmail
);