namespace PersonDataManagementSystem.Data.Entities;

public class Person
{
    public int Id { get; set; }
    public required string LastName { get; set; }
    public required string FirstName { get; set; }
    public DateOnly BirthDate { get; set; }

    public ICollection<Address> Addresses { get; set; } = [];
    public ICollection<Phone> Phones { get; set; } = [];
}
