namespace KodaStore.Api.DTOs;

public record ProductRequest
(
    string ProductCode,
    string Name,
    decimal Price,
    int Stock,
    string Category
);