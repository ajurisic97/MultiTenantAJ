using MultiTenantAJ.Application.Dto.PropertyManagement;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Mappings.PropertyManagement;

public static class GuestMappings
{
    public static GuestDto ToDto(Guest guest)
    {
        return new GuestDto
        {
            Id = guest.Id,
            FirstName = guest.FirstName,
            LastName = guest.LastName,
            Email = guest.Email,
            Phone = guest.Phone
        };
    }

    public static GuestDetailsDto ToDetailsDto(Guest guest, int reservationCount, int activeReservationCount)
    {
        return new GuestDetailsDto
        {
            Id = guest.Id,
            FirstName = guest.FirstName,
            LastName = guest.LastName,
            Email = guest.Email,
            Phone = guest.Phone,
            ReservationCount = reservationCount,
            ActiveReservationCount = activeReservationCount
        };
    }
}
