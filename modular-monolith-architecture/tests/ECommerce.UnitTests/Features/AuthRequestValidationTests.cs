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
        var type = request.GetType();
        foreach (var parameter in type.GetConstructors().Single().GetParameters())
        {
            var value = type.GetProperty(parameter.Name!, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase)!
                .GetValue(request);
            Validator.TryValidateValue(
                value,
                new ValidationContext(request) { MemberName = parameter.Name },
                errors,
                parameter.GetCustomAttributes(typeof(ValidationAttribute), inherit: true).Cast<ValidationAttribute>());
        }
        return errors;
    }
}
