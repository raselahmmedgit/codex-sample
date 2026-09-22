using System.ComponentModel.DataAnnotations;
using ECommerce.Application.Features.Auth.Models;

namespace ECommerce.UnitTests.Features;

public sealed class AuthRequestValidationTests
{
    [Fact]
    public void RegisterRequest_RejectsInvalidEmailAndShortPassword()
    {
        var request = new RegisterRequest("not-an-email", "short", "Customer");

        var errors = Validate(request);

        Assert.Contains(errors, error => error.MemberNames.Contains(nameof(RegisterRequest.Email)));
        Assert.Contains(errors, error => error.MemberNames.Contains(nameof(RegisterRequest.Password)));
    }

    [Fact]
    public void RegisterRequest_AcceptsValidFields()
    {
        var request = new RegisterRequest("customer@example.com", "ValidPass1", "Customer");

        Assert.Empty(Validate(request));
    }

    [Fact]
    public void RefreshTokenRequest_RejectsMissingToken()
    {
        var request = new RefreshTokenRequest(string.Empty);

        Assert.NotEmpty(Validate(request));
    }

    private static List<ValidationResult> Validate(object request)
    {
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(request, new ValidationContext(request), errors, validateAllProperties: true);
        return errors;
    }
}
