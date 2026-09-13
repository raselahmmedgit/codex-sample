using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Models;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Features.Customers;

public sealed record AddressRequest(string Line1, string? Line2, string City, string? State, string? PostalCode, string Country, bool IsDefault);
public sealed record AddressDto(Guid Id, string Line1, string? Line2, string City, string? State, string? PostalCode, string Country, bool IsDefault);

public sealed class CustomerService(IAddressRepository addressRepository)
{
    public async Task<Result<IReadOnlyCollection<AddressDto>>> ListAddressesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var addresses = await addressRepository.ListAsync(userId, cancellationToken);
        return Result<IReadOnlyCollection<AddressDto>>.Success(addresses.Select(Map).ToArray());
    }

    public async Task<Result<AddressDto>> CreateAddressAsync(Guid userId, AddressRequest request, CancellationToken cancellationToken)
    {
        var address = new Address(userId, request.Line1, request.City, request.Country);
        address.Update(request.Line1, request.Line2, request.City, request.State, request.PostalCode, request.Country);
        if (request.IsDefault) address.MarkAsDefault();
        await addressRepository.AddAsync(address, cancellationToken); await addressRepository.SaveChangesAsync(cancellationToken);
        return Result<AddressDto>.Success(Map(address), "Address created successfully.");
    }

    public async Task<Result<AddressDto>> UpdateAddressAsync(Guid userId, Guid addressId, AddressRequest request, CancellationToken cancellationToken)
    {
        var address = await addressRepository.GetByIdAsync(userId, addressId, cancellationToken);
        if (address is null) return Result<AddressDto>.Failure("Address was not found.");
        address.Update(request.Line1, request.Line2, request.City, request.State, request.PostalCode, request.Country);
        if (request.IsDefault) address.MarkAsDefault();
        await addressRepository.SaveChangesAsync(cancellationToken);
        return Result<AddressDto>.Success(Map(address), "Address updated successfully.");
    }

    public async Task<Result<bool>> DeleteAddressAsync(Guid userId, Guid addressId, CancellationToken cancellationToken)
    {
        var address = await addressRepository.GetByIdAsync(userId, addressId, cancellationToken);
        if (address is null) return Result<bool>.Failure("Address was not found.");
        addressRepository.Remove(address); await addressRepository.SaveChangesAsync(cancellationToken); return Result<bool>.Success(true);
    }

    private static AddressDto Map(Address x) => new(x.Id, x.Line1, x.Line2, x.City, x.State, x.PostalCode, x.Country, x.IsDefault);
}
