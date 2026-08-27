using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Api.Authorization;
using MultiTenantAJ.Api.Contracts.Catalog.Products;
using MultiTenantAJ.Application.Catalog.Products.Create;
using MultiTenantAJ.Application.Catalog.Products.Delete;
using MultiTenantAJ.Application.Catalog.Products.GetAll;
using MultiTenantAJ.Application.Catalog.Products.GetById;
using MultiTenantAJ.Application.Catalog.Products.Update;
using MultiTenantAJ.Domain.Authorization;

namespace MultiTenantAJ.Api.Controllers.Catalog;

[Route("api/[controller]")]
public class ProductsController : ApiControllerBase
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    [MustHavePermission(ActionCatalog.Search, ResourceCatalog.Products)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllProductQuery();
        var result = await _sender.Send(query);

        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.View, ResourceCatalog.Products)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetByIdProductQuery(id);
        var result = await _sender.Send(query);
        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.Create, ResourceCatalog.Products)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        var command = new CreateProductCommand(request.Name, request.Price);
        var result = await _sender.Send(command);

        return ResolveResult(result);
    }


    [MustHavePermission(ActionCatalog.Update, ResourceCatalog.Products)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateProductRequest request)
    {
        var command = new UpdateProductCommand(
                id,
                request.Name,
                request.Price);
        var result = await _sender.Send(command);

        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.Delete, ResourceCatalog.Products)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteProductCommand(id);
        var result = await _sender.Send(command);
        return ResolveResult(result);
    }
}
