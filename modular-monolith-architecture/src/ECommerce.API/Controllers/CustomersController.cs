using System.Security.Claims;
using ECommerce.Application.Features.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController, Authorize]
[Route("api/customers")]
public sealed class CustomersController(CustomerService customerService) : ControllerBase
{
    [HttpGet("addresses")]
    public async Task<IActionResult> ListAddresses(CancellationToken cancellationToken) => Ok(await customerService.ListAddressesAsync(UserId, cancellationToken));
    [HttpPost("addresses")]
    public async Task<IActionResult> CreateAddress(AddressRequest request, CancellationToken cancellationToken) => Ok(await customerService.CreateAddressAsync(UserId, request, cancellationToken));
    [HttpPut("addresses/{id:guid}")]
    public async Task<IActionResult> UpdateAddress(Guid id, AddressRequest request, CancellationToken cancellationToken) => Ok(await customerService.UpdateAddressAsync(UserId, id, request, cancellationToken));
    [HttpDelete("addresses/{id:guid}")]
    public async Task<IActionResult> DeleteAddress(Guid id, CancellationToken cancellationToken) => Ok(await customerService.DeleteAddressAsync(UserId, id, cancellationToken));
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
