using Ardalis.Specification;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Properties.Specifications;

public class PropertyByIdSpec : SingleResultSpecification<Property>
{
    public PropertyByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
