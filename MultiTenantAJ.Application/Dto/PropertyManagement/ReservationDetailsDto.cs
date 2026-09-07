using MultiTenantAJ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Dto.PropertyManagement;

public class ReservationDetailsDto
{
    public Guid Id { get; set; }

    public PropertyDto Property { get; set; } = default!;
    public GuestDto Guest { get; set; } = default!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfGuests { get; set; }
    public ReservationStatusEnum Status { get; set; }
}
