using MultiTenantAJ.Domain.Multitenancy;

namespace MultiTenantAJ.Domain.Models.PropertyManagement;

public class Property : IMustHaveTenant
{
    public Guid Id { get; private set; }
    public string TenantId { get; set; } = default!;
    public string Name { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public string? Description { get; private set; }
    public int NumberOfRooms { get; private set; }
    public int NumberOfBeds { get; private set; }
    public int MaximumCapacity { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<Reservation> Reservations { get; private set; } = [];

    public ICollection<MaintenanceRequest> MaintenanceRequests { get; private set; } = [];

    #region const
    public const int NameMaxLength = 150;
    public const int AddressMaxLength = 250;
    public const int DescriptionMaxLength = 1000;
    #endregion

    public Property()
    {
    }

    private Property(string name, string address,  string? description, int numberOfRooms, int numberOfBeds, int maximumCapacity)
    {
        Id = Guid.NewGuid();
        Name = name;
        Address = address;
        Description = description;
        NumberOfRooms = numberOfRooms;
        NumberOfBeds = numberOfBeds;
        MaximumCapacity = maximumCapacity;
        IsActive = true;
    }

    public static Property Create(string name, string address, string? description, int numberOfRooms, int numberOfBeds, int maximumCapacity)
    {
        return new Property(name, address, description, numberOfRooms, numberOfBeds, maximumCapacity);
    }

    public void Update(string name, string address, string? description, int numberOfRooms, int numberOfBeds, int maximumCapacity)
    {
        Name = name;
        Address = address;
        Description = description;
        NumberOfRooms = numberOfRooms;
        NumberOfBeds = numberOfBeds;
        MaximumCapacity = maximumCapacity;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}