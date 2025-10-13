using Company.Backend.Repositories.Interfaces;
using Employees.Backend.Data;
using Employees.Backend.Helpers;
using Employees.Backend.Repositories.Implementations;
using Employees.Shared.DTOs;
using Employees.Shared.Entities;
using Employees.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Company.Backend.Repositories.Implementations;

public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    private readonly DataContext _Context;

    public EmployeeRepository(DataContext context) : base(context)
    {
        _Context = context;
    }

    public override async Task<ActionResponse<Employee>> AddAsync(Employee entity)
    {
        if (entity.HireDate == default)
        {
            entity.HireDate = DateTime.Now;
        }

        _Context.Employees.Add(entity);
        await _Context.SaveChangesAsync();

        return new ActionResponse<Employee>
        {
            WasSuccess = true,
            Result = entity
        };
    }

    public override async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _Context.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x =>
            x.FirstName.Contains(pagination.Filter, StringComparison.CurrentCultureIgnoreCase) ||
                x.LastName.Contains(pagination.Filter, StringComparison.CurrentCultureIgnoreCase));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public override async Task<ActionResponse<IEnumerable<Employee>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _Context.Employees
                 .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            var filter = pagination.Filter.ToLower();
            queryable = queryable.Where(x =>
                x.FirstName.Contains(pagination.Filter, StringComparison.CurrentCultureIgnoreCase) ||
                x.LastName.Contains(pagination.Filter, StringComparison.CurrentCultureIgnoreCase));
        }

        return new ActionResponse<IEnumerable<Employee>>
        {
            WasSuccess = true,
            Result = await queryable
            .OrderBy(e => e.Id)
            .ThenBy(e => e.FirstName)
            .Paginate(pagination)
            .ToListAsync()
        };
    }

    public override async Task<ActionResponse<IEnumerable<Employee>>> SearchAsync(string query)
    {
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new ActionResponse<IEnumerable<Employee>>
                {
                    Message = "Debe ingresar un texto de búsqueda."
                };
            }

            var entities = await _Context.Set<Employee>()
                .Where(x => EF.Property<string>(x, "Name").Contains(query)
                         || EF.Property<string>(x, "LastName").Contains(query))
                .ToListAsync();

            if (!entities.Any())
            {
                return new ActionResponse<IEnumerable<Employee>>
                {
                    Message = "No se encontraron registros con ese criterio."
                };
            }

            return new ActionResponse<IEnumerable<Employee>>
            {
                WasSuccess = true,
                Result = entities
            };
        }
    }
}