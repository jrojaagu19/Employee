using Employees.Shared.Entities;

namespace Employees.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;

    public SeedDb(DataContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckEmployeeAsync();
    }

    private async Task CheckEmployeeAsync()
    {
        if (!_context.Employees.Any())
        {
            _context.Employees.Add(new Employee
            {
                FirstName = "Camilo",
                LastName = "Sanchez",
                IsActive = true,
                HireDate = DateTime.UtcNow,
                Salary = 3500000
            });
            _context.Employees.Add(new Employee
            {
                FirstName = "Franco",
                LastName = "Giraldo",
                IsActive = true,
                HireDate = DateTime.UtcNow,
                Salary = 1370000
            });
            _context.Employees.Add(new Employee
            {
                FirstName = "Sebastian",
                LastName = "Jaramillo",
                IsActive = true,
                HireDate = DateTime.UtcNow,
                Salary = 2570000
            });
            _context.Employees.Add(new Employee
            {
                FirstName = "Sandra",
                LastName = "Mira",
                IsActive = true,
                HireDate = DateTime.UtcNow,
                Salary = 1450000
            });
            _context.Employees.Add(new Employee
            {
                FirstName = "Santiago",
                LastName = "Robles",
                IsActive = true,
                HireDate = DateTime.UtcNow,
                Salary = 1700000
            });
            _context.Employees.Add(new Employee
            {
                FirstName = "Julio",
                LastName = "Perez",
                IsActive = true,
                HireDate = DateTime.UtcNow,
                Salary = 2500000
            });
            _context.Employees.Add(new Employee
            {
                FirstName = "Juliana",
                LastName = "Castro",
                IsActive = true,
                HireDate = DateTime.UtcNow,
                Salary = 1850000
            });
            _context.Employees.Add(new Employee
            {
                FirstName = "Julian",
                LastName = "Torres",
                IsActive = true,
                HireDate = DateTime.UtcNow,
                Salary = 15600000
            });
            _context.Employees.Add(new Employee
            {
                FirstName = "Carlos",
                LastName = "Guisao",
                IsActive = true,
                HireDate = DateTime.UtcNow,
                Salary = 1450000
            });
            _context.Employees.Add(new Employee
            {
                FirstName = "Juan",
                LastName = "Rojas",
                IsActive = true,
                HireDate = DateTime.UtcNow,
                Salary = 4500000
            });
            await _context.SaveChangesAsync();
        }
    }
}