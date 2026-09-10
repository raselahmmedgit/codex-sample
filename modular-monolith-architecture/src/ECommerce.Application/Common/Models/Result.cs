namespace ECommerce.Application.Common.Models;

public sealed record Result<T>(bool Succeeded, T? Data, string Message, IReadOnlyCollection<string> Errors)
{
    public static Result<T> Success(T data, string message = "Operation completed successfully.") =>
        new(true, data, message, Array.Empty<string>());

    public static Result<T> Failure(string message, params string[] errors) =>
        new(false, default, message, errors);
}
