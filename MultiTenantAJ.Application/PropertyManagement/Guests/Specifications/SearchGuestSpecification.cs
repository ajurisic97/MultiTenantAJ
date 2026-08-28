using Ardalis.Specification;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Guests.Specifications;

public class SearchGuestSpec : Specification<Guest>
{
    public SearchGuestSpec(string? firstName, string? lastName, string? email, string? phone)
    {
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            var normalizedFirstName = firstName.Trim().ToLower();

            Query.Where(x => x.FirstName.ToLower().Contains(normalizedFirstName));
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            var normalizedLastName = lastName.Trim().ToLower();

            Query.Where(x => x.LastName.ToLower().Contains(normalizedLastName));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            var normalizedEmail = email.Trim().ToLower();

            Query.Where(x => x.Email != null && x.Email.ToLower().Contains(normalizedEmail));
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            var normalizedPhone = phone.Trim();

            Query.Where(x => x.Phone != null && x.Phone.Contains(normalizedPhone));
        }

        Query.OrderBy(x => x.LastName)
            .ThenBy(x => x.FirstName);
    }
}
