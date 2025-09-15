using Microsoft.EntityFrameworkCore;
using Employees.Shared.Entities;

namespace Employees.Backend.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Employee>().HasIndex(x => x.FirstName);
        modelBuilder.Entity<Employee>().Property(x => x.Salary).HasColumnType("decimal(18,2)");
    }
}