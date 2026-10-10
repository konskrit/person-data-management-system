namespace PersonDataManagementSystem.Application.Dtos;

public record PersonListItemDto(
    int Id,
    string LastName,
    string FirstName,
    DateOnly BirthDate);
