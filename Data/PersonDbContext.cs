using Microsoft.EntityFrameworkCore;
using PersonDataManagementSystem.Data.Entities;

namespace PersonDataManagementSystem.Data;

public class PersonDbContext(DbContextOptions<PersonDbContext> options) : DbContext(options)
{
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Phone> Phones => Set<Phone>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().ToTable("Person");
        modelBuilder.Entity<Address>().ToTable("Address");
        modelBuilder.Entity<Phone>().ToTable("Phone");

        modelBuilder.Entity<Address>()
            .HasOne(x => x.Person)
            .WithMany(x => x.Addresses)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Phone>()
            .HasOne(x => x.Person)
            .WithMany(x => x.Phones)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
