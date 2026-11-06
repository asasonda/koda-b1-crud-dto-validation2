using KodaStore.Api.Models;

namespace KodaStore.Api.DTOs;

public sealed record ProductResponse(string ProductCode, string Name, decimal Price, int Stock, string Category)
{
    public static ProductResponse FromModel(Products products) => new(products.ProductCode, products.Name, products.Price, products.Stock, products.Category);
}