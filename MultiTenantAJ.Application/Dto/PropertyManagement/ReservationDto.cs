using MultiTenantAJ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Dto.PropertyManagement;

public class ReservationDto
{
    public Guid Id { get; set; }

    public Guid PropertyId { get; set; }
    public string PropertyName { get; set; } = default!;

    public Guid GuestId { get; set; }
    public string GuestName { get; set; } = default!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfGuests { get; set; }
    public ReservationStatusEnum Status { get; set; }
}
