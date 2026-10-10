using Microsoft.EntityFrameworkCore;
using PersonDataManagementSystem.Application.Dtos;
using PersonDataManagementSystem.Application.Interfaces;
using PersonDataManagementSystem.Data;

namespace PersonDataManagementSystem.Application.Services;

public class PersonService(PersonDbContext db) : IPersonService
{
    public async Task<IReadOnlyList<PersonListItemDto>> GetPersonsAsync(
        string? name,
        CancellationToken cancellationToken = default)
    {
        var query = db.Persons.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(name))
        {
            var term = name.Trim();
            query = query.Where(p =>
                p.LastName.Contains(term) ||
                p.FirstName.Contains(term));
        }

        return await query
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .Select(p => new PersonListItemDto(p.Id, p.LastName, p.FirstName, p.BirthDate))
            .ToListAsync(cancellationToken);
    }

    public async Task<PersonDetailDto?> GetPersonByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await db.Persons
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new PersonDetailDto(
                p.Id,
                p.LastName,
                p.FirstName,
                p.BirthDate,
                p.Addresses
                    .Select(a => new AddressDto(a.Id, a.PostalCode, a.City, a.Street, a.HouseNumber))
                    .ToList(),
                p.Phones
                    .Select(ph => new PhoneDto(ph.Id, ph.Number))
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PersonDetailDto?> UpdatePersonNameAsync(
        int id,
        UpdatePersonNameDto request,
        CancellationToken cancellationToken = default)
    {
        var person = await db.Persons.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (person is null)
            return null;

        person.LastName = request.LastName.Trim();
        person.FirstName = request.FirstName.Trim();
        await db.SaveChangesAsync(cancellationToken);

        return await GetPersonByIdAsync(id, cancellationToken);
    }

    public async Task<bool> DeletePersonAsync(int id, CancellationToken cancellationToken = default)
    {
        var person = await db.Persons.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (person is null)
            return false;

        db.Persons.Remove(person);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
