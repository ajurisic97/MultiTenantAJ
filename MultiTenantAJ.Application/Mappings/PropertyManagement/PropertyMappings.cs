using MultiTenantAJ.Application.Dto.PropertyManagement;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Mappings.PropertyManagement;
public static class PropertyMappings
{
    public static PropertyDto ToDto(Property property)
    {
        return new PropertyDto
        {
            Id = property.Id,
            Name = property.Name,
            Address = property.Address,
            MaximumCapacity = property.MaximumCapacity,
            IsActive = property.IsActive,
        };
    }

    public static PropertyDetailsDto ToDetailsDto(Property property, int reservationCount, int activeReservationCount, int activeMaintenanceCount)
    {
        return new PropertyDetailsDto
        {
            Id = property.Id,
            Name = property.Name,
            Address = property.Address,
            Description = property.Description,
            NumberOfRooms = property.NumberOfRooms,
            NumberOfBeds = property.NumberOfBeds,
            MaximumCapacity = property.MaximumCapacity,
            IsActive = property.IsActive,
            ReservationCount = reservationCount,
            ActiveReservationCount = activeReservationCount,
            ActiveMaintenanceRequestCount = activeMaintenanceCount
        };
    }
}
