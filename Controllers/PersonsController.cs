using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonDataManagementSystem.Application.Dtos;
using PersonDataManagementSystem.Application.Interfaces;

namespace PersonDataManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController(IPersonService personService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PersonListItemDto>>> GetPersons(
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var persons = await personService.GetPersonsAsync(name, cancellationToken);
        return Ok(persons);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonDetailDto>> GetPerson(
        int id,
        CancellationToken cancellationToken)
    {
        var person = await personService.GetPersonByIdAsync(id, cancellationToken);
        return person is null ? NotFound() : Ok(person);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PersonDetailDto>> UpdatePersonName(
        int id,
        [FromBody] UpdatePersonNameDto request,
        CancellationToken cancellationToken)
    {
        var person = await personService.UpdatePersonNameAsync(id, request, cancellationToken);
        return person is null ? NotFound() : Ok(person);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePerson(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await personService.DeletePersonAsync(id, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("Person still has addresses or phone numbers.");
        }
    }
}
