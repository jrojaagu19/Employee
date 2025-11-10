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
        //await _context.Database.MigrateAsync();
        await _context.Database.EnsureCreatedAsync();

        await CheckEmployeeFullAsync();
        await CheckCountriesFullAsync();
        await CheckEmployeeAsync();
        await CheckCountriesAsync();
        await CheckCategoriesAsync();
    }

    private async Task CheckEmployeeFullAsync()
    {
        if (!_context.Employees.Any())
        {
            var sqlFilePath = "Data\\EmployeesDB.sql";

            if (File.Exists(sqlFilePath))
            {
                var employeesSQLScript = File.ReadAllText(sqlFilePath);
                await _context.Database.ExecuteSqlRawAsync(employeesSQLScript);
            }
        }
    }

    private async Task CheckCountriesFullAsync()
    {
        if (!_context.Countries.Any())
        {
            var sqlFilePath = File.ReadAllText("Data\\CountriesStatesCities.sql");

            if (File.Exists(sqlFilePath))
            {
                var countriesSQLScript = File.ReadAllText(sqlFilePath);
                await _context.Database.ExecuteSqlRawAsync(countriesSQLScript);
            }
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
                new() { FirstName = "Ana", LastName = "Maru", IsActive = false, HireDate = new DateTime(2017, 2, 14), Salary = 2800000 },
                new() { FirstName = "Carla", LastName = "López", IsActive = true, HireDate = new DateTime(2022, 4, 25), Salary = 2600000 },
                new() { FirstName = "Uribe", LastName = "Sanchez", IsActive = true, HireDate = new DateTime(2020, 9, 30), Salary = 3500000 },
                new() { FirstName = "Petro", LastName = "González", IsActive = true, HireDate = new DateTime(2019, 11, 12), Salary = 3400000 },
                new() { FirstName = "Alvaro", LastName = "Pérez", IsActive = true, HireDate = new DateTime(2021, 1, 5), Salary = 1400000 },
                new() { FirstName = "Carlo", LastName = "Sánchez", IsActive = true, HireDate = new DateTime(2018, 6, 18), Salary = 9600000 },
            };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckCategoriesAsync()
    {
        if (!_context.Categories.Any())
        {
            _context.Categories.Add(new Category { FirstName = "Apple" });
            _context.Categories.Add(new Category { FirstName = "Autos" });
            _context.Categories.Add(new Category { FirstName = "Belleza" });
            _context.Categories.Add(new Category { FirstName = "Calzado" });
            _context.Categories.Add(new Category { FirstName = "Comida" });
            _context.Categories.Add(new Category { FirstName = "Cosmeticos" });
            _context.Categories.Add(new Category { FirstName = "Deportes" });
            _context.Categories.Add(new Category { FirstName = "Erótica" });
            _context.Categories.Add(new Category { FirstName = "Ferreteria" });
            _context.Categories.Add(new Category { FirstName = "Gamer" });
            _context.Categories.Add(new Category { FirstName = "Hogar" });
            _context.Categories.Add(new Category { FirstName = "Jardín" });
            _context.Categories.Add(new Category { FirstName = "Jugetes" });
            _context.Categories.Add(new Category { FirstName = "Lenceria" });
            _context.Categories.Add(new Category { FirstName = "Mascotas" });
            _context.Categories.Add(new Category { FirstName = "Nutrición" });
            _context.Categories.Add(new Category { FirstName = "Ropa" });
            _context.Categories.Add(new Category { FirstName = "Tecnología" });
            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckCountriesAsync()
    {
        if (!_context.Countries.Any())
        {
            _context.Countries.Add(new Country
            {
                FirstName = "Colombia",
                States = [
                    new State()
                    {
                        FirstName = "Antioquia",
                        Cities = [
                            new City() { FirstName = "Medellín" },
                            new City() { FirstName = "Itagüí" },
                            new City() { FirstName = "Envigado" },
                            new City() { FirstName = "Bello" },
                            new City() { FirstName = "Rionegro" },
                        ]
                    },
                    new State()
                    {
                        FirstName = "Bogotá",
                        Cities = [
                            new City() { FirstName = "Usaquen" },
                            new City() { FirstName = "Champinero" },
                            new City() { FirstName = "Santa fe" },
                            new City() { FirstName = "Useme" },
                            new City() { FirstName = "Bosa" },
                        ]
                    },
                ]
            });
            _context.Countries.Add(new Country
            {
                FirstName = "Estados Unidos",
                States = [
                    new State()
                {
                    FirstName = "Florida",
                    Cities = [
                        new City() { FirstName = "Orlando" },
                        new City() { FirstName = "Miami" },
                        new City() { FirstName = "Tampa" },
                        new City() { FirstName = "Fort Lauderdale" },
                        new City() { FirstName = "Key West" },
                    ]
                },
                new State()
                    {
                        FirstName = "Texas",
                        Cities = [
                            new City() { FirstName = "Houston" },
                            new City() { FirstName = "San Antonio" },
                            new City() { FirstName = "Dallas" },
                            new City() { FirstName = "Austin" },
                            new City() { FirstName = "El Paso" },
                        ]
                    },
                ]
            });
        }
        await _context.SaveChangesAsync();
    }
}