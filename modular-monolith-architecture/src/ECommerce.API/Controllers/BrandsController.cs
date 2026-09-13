using ECommerce.Application.Features.Catalog;
using ECommerce.Application.Features.Catalog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/brands")]
public sealed class BrandsController(ICatalogService catalogService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> List(CancellationToken cancellationToken) => Ok(await catalogService.GetBrandsAsync(cancellationToken));

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create(CreateBrandRequest request, CancellationToken cancellationToken) => Ok(await catalogService.CreateBrandAsync(request, cancellationToken));

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(Guid id, UpdateBrandRequest request, CancellationToken cancellationToken) => Ok(await catalogService.UpdateBrandAsync(id, request, cancellationToken));

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken) => Ok(await catalogService.DeleteBrandAsync(id, cancellationToken));
}
