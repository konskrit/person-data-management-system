namespace PersonDataManagementSystem.Data.Entities;

public class Phone
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public required string Number { get; set; }

    public required Person Person { get; set; }
}
