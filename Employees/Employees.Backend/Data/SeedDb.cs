using Employees.Shared.Entities;
using Microsoft.EntityFrameworkCore;

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
        await CheckEmployeeFullAsync();
        await CheckEmployeeAsync();
    }

    private async Task CheckEmployeeFullAsync()
    {
        if (!_context.Employees.Any())
        {
            var employeesSQLScript = File.ReadAllText("Data\\EmployeesDB.sql");
            await _context.Database.ExecuteSqlRawAsync(employeesSQLScript);
        }
    }

    private async Task CheckEmployeeAsync()
    {
        if (!_context.Employees.Any())
        {
            var employees = new List<Employee>
            {
                new() { FirstName = "Siri", LastName = "Cardona", IsActive = true, HireDate = new DateTime(2022, 1, 30), Salary = 1500000 },
                new() { FirstName = "Marta", LastName = "Conca", IsActive = true, HireDate = new DateTime(2019, 3, 20), Salary = 3200000 },
                new() { FirstName = "Carlos", LastName = "Ramon", IsActive = true, HireDate = new DateTime(2021, 5, 10), Salary = 2000000 },
                new() { FirstName = "Cristian", LastName = "García", IsActive = true, HireDate = new DateTime(2018, 7, 8), Salary = 1040000 },
                new() { FirstName = "Ana", LastName = "maru", IsActive = false, HireDate = new DateTime(2017, 2, 14), Salary = 2800000 },
                new() { FirstName = "Carla", LastName = "López", IsActive = true, HireDate = new DateTime(2022, 4, 25), Salary = 2600000 },
                new() { FirstName = "Uribe", LastName = "sanchez", IsActive = true, HireDate = new DateTime(2020, 9, 30), Salary = 3500000 },
                new() { FirstName = "Petro", LastName = "González", IsActive = true, HireDate = new DateTime(2019, 11, 12), Salary = 3400000 },
                new() { FirstName = "Alvaro", LastName = "Pérez", IsActive = true, HireDate = new DateTime(2021, 1, 5), Salary = 1400000 },
                new() { FirstName = "Carlo", LastName = "Sánchez", IsActive = true, HireDate = new DateTime(2018, 6, 18), Salary = 9600000 },
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();
        }
    }
}