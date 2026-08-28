using Ardalis.Specification;
using MultiTenantAJ.Application.Dto.PropertyManagement;
using MultiTenantAJ.Domain.Enums;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MultiTenantAJ.Application.PropertyManagement.Properties.Specifications;
public class SearchPropertySpec : Specification<Property>
{
    public SearchPropertySpec(string? name, string? address, bool? isActive, int? minimumCapacity)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            var normalizedName = name.Trim().ToLower();

            Query.Where(x => x.Name.ToLower().Contains(normalizedName));
        }

        if (!string.IsNullOrWhiteSpace(address))
        {
            var normalizedAddress = address.Trim().ToLower();

            Query.Where(x => x.Address.ToLower().Contains(normalizedAddress));
        }

        if (isActive.HasValue)
        {
            Query.Where(x => x.IsActive == isActive.Value);
        }

        if (minimumCapacity.HasValue)
        {
            Query.Where(x => x.MaximumCapacity >= minimumCapacity.Value);
        }

        Query.OrderBy(x => x.Name);
    }
}