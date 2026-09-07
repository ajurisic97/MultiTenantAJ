namespace MultiTenantAJ.Api.Contracts.PropertyManagement.Properties;
public class CreatePropertyRequest
{
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string? Description { get; set; }
    public int NumberOfRooms { get; set; }
    public int NumberOfBeds { get; set; }
    public int MaximumCapacity { get; set; }
}
