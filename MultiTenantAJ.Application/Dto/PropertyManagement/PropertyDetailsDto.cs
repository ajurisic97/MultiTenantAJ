using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Dto.PropertyManagement;
public class PropertyDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string? Description { get; set; }
    public int NumberOfRooms { get; set; }
    public int NumberOfBeds { get; set; }
    public int MaximumCapacity { get; set; }
    public bool IsActive { get; set; }

    public int ReservationCount { get; set; }
    public int ActiveReservationCount { get; set; }
    public int ActiveMaintenanceRequestCount { get; set; }
}
