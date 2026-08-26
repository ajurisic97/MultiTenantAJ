using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Api.Contracts.Catalog.Products;
using MultiTenantAJ.Application.Catalog.Products.Create;
using MultiTenantAJ.Application.Catalog.Products.Delete;
using MultiTenantAJ.Application.Catalog.Products.GetAll;
using MultiTenantAJ.Application.Catalog.Products.GetById;
using MultiTenantAJ.Application.Catalog.Products.Update;
using MultiTenantAJ.Domain.Multitenancy;

namespace MultiTenantAJ.Api.Controllers.Catalog;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var products = await _sender.Send(
            new GetProductsQuery(),
            cancellationToken);

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromHeader(Name = MultitenancyConstants.TenantIdName)] string tenant,
        CancellationToken cancellationToken)
    {
        var product = await _sender.Send(new GetProductByIdQuery(id), cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromHeader(Name = MultitenancyConstants.TenantIdName)] string tenant,
        CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(request.Name, request.Price);

        var productId = await _sender.Send(
            command,
            cancellationToken);

        return Ok(productId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
    Guid id,
    [FromHeader(Name = MultitenancyConstants.TenantIdName)] string tenant,
    UpdateProductRequest request,
    CancellationToken cancellationToken)
    {
        var productId = await _sender.Send(
            new UpdateProductCommand(
                id,
                request.Name,
                request.Price),
            cancellationToken);

        if (productId is null)
        {
            return NotFound();
        }

        return Ok(productId);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromHeader(Name = MultitenancyConstants.TenantIdName)] string tenant,
        CancellationToken cancellationToken)
    {
        var productId = await _sender.Send(
            new DeleteProductCommand(id),
            cancellationToken);

        if (productId is null)
        {
            return NotFound();
        }

        return Ok(productId);
    }
}
