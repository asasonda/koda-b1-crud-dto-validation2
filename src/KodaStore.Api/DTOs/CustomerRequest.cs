using System.ComponentModel.DataAnnotations;

namespace KodaStore.Api.DTOs;

public record CustomersRequest(
    [Required]
    string CustomerCode,

    [Required]
    string Name,

    [Required]
    string Email,

    [Required]
    string Phone,

    [Required]
    string Address
);