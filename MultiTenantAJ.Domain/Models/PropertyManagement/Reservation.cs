using MultiTenantAJ.Domain.Enums;
using MultiTenantAJ.Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Models.PropertyManagement;

public class Reservation : IMustHaveTenant
{
    public Guid Id { get; private set; }
    public string TenantId { get; set; } = default!;
    public Guid PropertyId { get; private set; }
    public Guid GuestId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int NumberOfGuests { get; private set; }
    public ReservationStatusEnum Status { get; private set; }

    public Property Property { get; private set; } = default!;
    public Guest Guest { get; private set; } = default!;



    public Reservation()
    {
    }

    private Reservation(Guid propertyId, Guid guestId, DateTime startDate, DateTime endDate, int numberOfGuests)
    {
        Id = Guid.NewGuid();
        PropertyId = propertyId;
        GuestId = guestId;
        StartDate = startDate;
        EndDate = endDate;
        NumberOfGuests = numberOfGuests;
        Status = ReservationStatusEnum.Pending;
    }

    public static Reservation Create(Guid propertyId, Guid guestId, DateTime startDate, DateTime endDate, int numberOfGuests)
    {
        return new Reservation(propertyId, guestId, startDate, endDate, numberOfGuests);
    }

    public void Update(Guid propertyId, Guid guestId, DateTime startDate, DateTime endDate, int numberOfGuests)
    {
        PropertyId = propertyId;
        GuestId = guestId;
        StartDate = startDate;
        EndDate = endDate;
        NumberOfGuests = numberOfGuests;
    }

    public void Confirm()
    {
        Status = ReservationStatusEnum.Confirmed;
    }

    public void Cancel()
    {
        Status = ReservationStatusEnum.Cancelled;
    }

    public void Complete()
    {
        Status = ReservationStatusEnum.Completed;
    }
}
