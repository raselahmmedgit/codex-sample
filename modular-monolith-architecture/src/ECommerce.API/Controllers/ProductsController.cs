using ECommerce.Application.Features.Products.Models;
using ECommerce.Application.Features.Products.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> List([FromQuery] ProductFilterRequest request, CancellationToken cancellationToken) => Ok(await productService.SearchAsync(request, cancellationToken));

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken) => Ok(await productService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    [Authorize(Roles = "Admin,Manager,Seller")]
    public async Task<IActionResult> Create(CreateProductRequest request, CancellationToken cancellationToken) => Ok(await productService.CreateAsync(request, cancellationToken));

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Manager,Seller")]
    public async Task<IActionResult> Update(Guid id, UpdateProductRequest request, CancellationToken cancellationToken) => Ok(await productService.UpdateAsync(id, request, cancellationToken));

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken) => Ok(await productService.DeleteAsync(id, cancellationToken));

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> ChangeStatus(Guid id, ChangeProductStatusRequest request, CancellationToken cancellationToken) => Ok(await productService.ChangeStatusAsync(id, request, cancellationToken));
}
