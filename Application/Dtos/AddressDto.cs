namespace PersonDataManagementSystem.Application.Dtos;

public record AddressDto(
    int Id,
    string PostalCode,
    string City,
    string Street,
    string HouseNumber);
