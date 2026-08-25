using Ardalis.Specification;
using MultiTenantAJ.Domain.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.Specifications;

public class ProductByIdSpec : SingleResultSpecification<Product>
{
    public ProductByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
