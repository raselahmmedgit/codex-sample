namespace ECommerce.Application.Common.Exceptions;

public abstract class ECommerceException(string message) : Exception(message);
public sealed class NotFoundException(string message) : ECommerceException(message);
public sealed class ConflictException(string message) : ECommerceException(message);
public sealed class ValidationException(string message) : ECommerceException(message);
public sealed class UnauthorizedException(string message) : ECommerceException(message);
public sealed class ForbiddenException(string message) : ECommerceException(message);
