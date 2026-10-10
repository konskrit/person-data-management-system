using PersonDataManagementSystem.Application.Dtos;

namespace PersonDataManagementSystem.Application.Interfaces;

public interface IPersonService
{
    Task<IReadOnlyList<PersonListItemDto>> GetPersonsAsync(string? name, CancellationToken cancellationToken = default);
    Task<PersonDetailDto?> GetPersonByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PersonDetailDto?> UpdatePersonNameAsync(int id, UpdatePersonNameDto request, CancellationToken cancellationToken = default);
    Task<bool> DeletePersonAsync(int id, CancellationToken cancellationToken = default);
}
