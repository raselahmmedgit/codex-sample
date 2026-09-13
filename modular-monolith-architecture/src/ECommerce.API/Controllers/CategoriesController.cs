using ECommerce.Application.Features.Catalog;
using ECommerce.Application.Features.Catalog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoriesController(ICatalogService catalogService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> List(CancellationToken cancellationToken) => Ok(await catalogService.GetCategoriesAsync(cancellationToken));

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create(CreateCategoryRequest request, CancellationToken cancellationToken) => Ok(await catalogService.CreateCategoryAsync(request, cancellationToken));

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken) => Ok(await catalogService.UpdateCategoryAsync(id, request, cancellationToken));

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken) => Ok(await catalogService.DeleteCategoryAsync(id, cancellationToken));
}
