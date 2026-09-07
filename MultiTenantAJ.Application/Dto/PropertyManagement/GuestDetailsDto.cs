using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Dto.PropertyManagement;

public class GuestDetailsDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public int ReservationCount { get; set; }
    public int ActiveReservationCount { get; set; }
}
