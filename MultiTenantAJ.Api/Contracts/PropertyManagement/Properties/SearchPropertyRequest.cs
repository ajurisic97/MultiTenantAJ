namespace MultiTenantAJ.Api.Contracts.PropertyManagement.Properties;

public class SearchPropertyRequest
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public bool? IsActive { get; set; }
    public int? MinimumCapacity { get; set; }
}