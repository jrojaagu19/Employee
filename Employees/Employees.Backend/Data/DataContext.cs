using Microsoft.EntityFrameworkCore;
using Employees.Shared.Entities;

namespace Employees.Backend.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; } = null!;
}