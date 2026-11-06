namespace KodaStore.Api.Files;

public sealed record StorageProductImage
(
    string StorageFileName,
    long SizeInBytes,
    string ContentType
);