using System.ComponentModel.DataAnnotations;

namespace KodaStore.Api.DTOs;

public record ProductRequest(
    [Required, StringLength(30)]
    string ProductCode, 

    [Required, StringLength(120)]
    string Name, 

    [Required]
    decimal Price,

    [Required, Range(0, int.MaxValue)] 
    int Stock, 

    [Required, StringLength(120)]
    string Category
);