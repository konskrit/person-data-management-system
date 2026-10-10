namespace PersonDataManagementSystem.Application.Dtos;

public record PersonDetailDto(
    int Id,
    string LastName,
    string FirstName,
    DateOnly BirthDate,
    IReadOnlyList<AddressDto> Addresses,
    IReadOnlyList<PhoneDto> Phones);
