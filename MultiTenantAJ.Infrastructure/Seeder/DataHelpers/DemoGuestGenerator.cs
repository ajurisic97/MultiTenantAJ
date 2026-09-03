using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Seeder.DataHelpers;

public static class DemoGuestGenerator
{
    public static List<Guest> Create(string tenantId)
    {
        var guestData = new[]
        {
            ("John", "Smith"),
            ("Emma", "Johnson"),
            ("Michael", "Brown"),
            ("Olivia", "Davis"),
            ("Daniel", "Wilson"),
            ("Sophia", "Miller"),
            ("James", "Anderson"),
            ("Emily", "Taylor")
        };

        var guests = new List<Guest>();

        for (var i = 0; i < guestData.Length; i++)
        {
            var guestDataItem = guestData[i];

            var guest = Guest.Create(
                guestDataItem.Item1,
                guestDataItem.Item2,
                $"{guestDataItem.Item1.ToLowerInvariant()}.{guestDataItem.Item2.ToLowerInvariant()}.{tenantId}@example.com",
                $"+385 91 555 {1000 + i}");

            guests.Add(guest);
        }

        return guests;
    }
}