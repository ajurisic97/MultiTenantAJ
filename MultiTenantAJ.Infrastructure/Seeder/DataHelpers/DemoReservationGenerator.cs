using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Seeder.DataHelpers;

public static class DemoReservationGenerator
{
    private const int ReservationCount = 12;

    public static List<Reservation> Create(
        IReadOnlyList<Property> properties,
        IReadOnlyList<Guest> guests)
    {
        if (properties.Count == 0 || guests.Count == 0)
        {
            return [];
        }

        var reservations = new List<Reservation>();
        var today = DateTime.UtcNow.Date;

        for (var i = 0; i < ReservationCount; i++)
        {
            var property = properties[i % properties.Count];
            var guest = guests[i % guests.Count];

            var startDate = today.AddDays(-120 + i * 20);
            var endDate = startDate.AddDays(4 + i % 3);

            var numberOfGuests =
                1 + i % property.MaximumCapacity;

            var reservation = Reservation.Create(
                property.Id,
                guest.Id,
                startDate,
                endDate,
                numberOfGuests);

            SetStatus(
                reservation,
                endDate,
                i,
                today);

            reservations.Add(reservation);
        }

        return reservations;
    }

    private static void SetStatus(
        Reservation reservation,
        DateTime endDate,
        int index,
        DateTime today)
    {
        if (endDate < today)
        {
            if (index % 4 == 0)
            {
                reservation.Cancel();
                return;
            }

            reservation.Complete();
            return;
        }

        if (index % 5 == 0)
        {
            reservation.Cancel();
            return;
        }

        if (index % 2 == 0)
        {
            reservation.Confirm();
        }
    }
}
