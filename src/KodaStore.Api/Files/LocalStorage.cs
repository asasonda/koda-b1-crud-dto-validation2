namespace KodaStore.Api.Files;

public sealed class LocalStorage : IProductImageStorage
{
    public Task<StorageProductImage> SaveAsync(string productCode, IFormFile file, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}