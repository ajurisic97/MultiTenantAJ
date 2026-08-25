namespace MultiTenantAJ.Api.Contracts.Catalog.Products;

public record CreateProductRequest(
    string Name,
    decimal Price);
