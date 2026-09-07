using MultiTenantAJ.Application.Dto.PropertyManagement;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Mappings.PropertyManagement;
public static class ReservationMappings
{
    public static ReservationDto ToDto(Reservation reservation)
    {
        return new ReservationDto
        {
            Id = reservation.Id,
            PropertyId = reservation.PropertyId,
            PropertyName = reservation.Property.Name,
            GuestId = reservation.GuestId,
            GuestName = $"{reservation.Guest.FirstName} {reservation.Guest.LastName}",
            StartDate = reservation.StartDate,
            EndDate = reservation.EndDate,
            NumberOfGuests = reservation.NumberOfGuests,
            Status = reservation.Status
        };
    }

    public static ReservationDetailsDto ToDetailsDto(Reservation reservation)
    {
        return new ReservationDetailsDto
        {
            Id = reservation.Id,
            Property = PropertyMappings.ToDto(reservation.Property),
            Guest = GuestMappings.ToDto(reservation.Guest),
            StartDate = reservation.StartDate,
            EndDate = reservation.EndDate,
            NumberOfGuests = reservation.NumberOfGuests,
            Status = reservation.Status
        };
    }
}