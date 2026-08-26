using MultiTenantAJ.Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Models.Catalog;

public class Product : IMustHaveTenant
{
    public Guid Id { get; private set; }

    public string TenantId { get; set; } = default!;

    public string Name { get; private set; } = default!;

    public decimal Price { get; private set; }
    public Product()
    {
        
    }
    private Product(string name, decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
    }
    public static Product Create(string name,decimal price)
    {
        return new Product(name, price);
    }

    public void Update(string name,decimal price)
    {
        Name = name;
        Price = price;
    }
}
