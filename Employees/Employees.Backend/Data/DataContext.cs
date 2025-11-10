using Microsoft.EntityFrameworkCore;
using Employees.Shared.Entities;

namespace Employees.Backend.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Category>().HasIndex(c => c.FirstName).IsUnique();
        modelBuilder.Entity<City>().HasIndex(c => new { c.StateId, c.FirstName }).IsUnique();
        modelBuilder.Entity<Country>().HasIndex(c => c.FirstName).IsUnique();
        modelBuilder.Entity<Employee>().HasIndex(x => x.FirstName);
        modelBuilder.Entity<State>().HasIndex(s => new { s.CountryId, s.FirstName }).IsUnique();
        modelBuilder.Entity<Employee>().Property(x => x.Salary).HasColumnType("decimal(18,2)");

        DisableCascadingDelete(modelBuilder);
    }

    private void DisableCascadingDelete(ModelBuilder modelBuilder)
    {
        var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
        foreach (var relationship in relationships)
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}