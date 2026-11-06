namespace KodaStore.Api.Files;

public interface IProductImageStorage
{
    Task<StorageProductImage> SaveAsync(string productCode, IFormFile file, CancellationToken ct);
}