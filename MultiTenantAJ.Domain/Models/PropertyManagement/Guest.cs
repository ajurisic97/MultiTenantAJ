using MultiTenantAJ.Domain.Multitenancy;

namespace MultiTenantAJ.Domain.Models.PropertyManagement;

public class Guest : IMustHaveTenant
{
    public Guid Id { get; private set; }

    public string TenantId { get; set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public ICollection<Reservation> Reservations { get; private set; } = [];

    #region const
    public const int FirstNameMaxLength = 100;
    public const int LastNameMaxLength = 100;
    public const int EmailMaxLength = 200;
    public const int PhoneMaxLength = 30;
    #endregion

    public Guest()
    {
    }

    private Guest(string firstName, string lastName, string? email, string? phone)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
    }

    public static Guest Create(string firstName, string lastName, string? email, string? phone)
    {
        return new Guest(firstName, lastName, email, phone);
    }

    public void Update(string firstName, string lastName, string? email, string? phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
    }
}
