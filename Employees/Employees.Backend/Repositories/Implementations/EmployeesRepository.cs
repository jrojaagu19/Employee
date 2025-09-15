using Employees.Backend.Data;
using Employees.Backend.Repositories.Interfaces;
using Employees.Shared.Entities;
using Employees.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Employees.Backend.Repositories.Implementations;

public class EmployeesRepository : GenericRepository<Employee>, IEmployeesRepository
{
    private readonly DataContext _context;

    public EmployeesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ActionResponse<IEnumerable<Employee>>> GetByNameAsync(string name)
    {
        var query = _context.Employees
            .Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name));

        return new ActionResponse<IEnumerable<Employee>>
        {
            WasSuccess = true,
            Result = await query.ToListAsync()
        };
    }
}