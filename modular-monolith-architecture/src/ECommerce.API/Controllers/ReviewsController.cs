using System.Security.Claims;
using ECommerce.Application.Features.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController, Authorize]
[Route("api/reviews")]
public sealed class ReviewsController(ReviewService reviewService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateReviewRequest request, CancellationToken cancellationToken) => Ok(await reviewService.CreateAsync(UserId, request, cancellationToken));
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
