namespace MultiTenantAJ.Api.Contracts.Catalog.Products;
public record UpdateProductRequest(
    string Name,
    decimal Price);
