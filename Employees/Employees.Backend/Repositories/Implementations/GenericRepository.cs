using Employees.Backend.Data;
using Employees.Backend.Helpers;
using Employees.Backend.Repositories.Interfaces;
using Employees.Shared.DTOs;
using Employees.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Employees.Backend.Repositories.Implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DataContext _context;
    private readonly DbSet<T> _entity;

    public GenericRepository(DataContext context)
    {
        _context = context;
        _entity = _context.Set<T>();
    }

    public virtual async Task<ActionResponse<T>> AddAsync(T entity)
    {
        _context.Add(entity);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<T>
            {
                WasSuccess = true,
                Result = entity
            };
        }
        catch (DbUpdateException)
        {
            return DbUpdateExceptionActionResponse();
        }
        catch (Exception exception)
        {
            return ExceptionActionResponse(exception);
        }
    }

    public virtual async Task<ActionResponse<T>> DeleteAsync(int id)
    {
        var row = await _entity.FindAsync(id);
        if (row == null)
        {
            return new ActionResponse<T>
            {
                Message = "No se encontró el registro."
            };
        }
        _entity.Remove(row);

        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<T>
            {
                WasSuccess = true,
            };
        }
        catch
        {
            return new ActionResponse<T>
            {
                Message = "No se puede borrar porque tiene registros relacionados."
            };
        }
    }

    public virtual async Task<ActionResponse<T>> GetAsync(int id)
    {
        var row = await _entity.FindAsync(id);
        if (row == null)
        {
            return new ActionResponse<T>
            {
                Message = "No se encontró el registro."
            };
        }
        return new ActionResponse<T>
        {
            WasSuccess = true,
            Result = row
        };
    }

    public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()
    {
        return new ActionResponse<IEnumerable<T>>
        {
            WasSuccess = true,
            Result = await _entity.ToListAsync()
        };
    }

    public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _entity.AsQueryable();

        return new ActionResponse<IEnumerable<T>>
        {
            WasSuccess = true,
            Result = await queryable
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public virtual async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _entity.AsQueryable();
        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public virtual async Task<ActionResponse<IEnumerable<T>>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new ActionResponse<IEnumerable<T>>
            {
                Message = "Debe ingresar un texto de búsqueda."
            };
        }

        var searchTerm = query.Trim();

        var entities = await _context.Set<T>()
            .Where(x =>
                EF.Functions.Like(EF.Property<string>(x, "FirstName"), $"{searchTerm}%") ||
                EF.Functions.Like(EF.Property<string>(x, "LastName"), $"{searchTerm}%"))
            .OrderBy(x => EF.Property<string>(x, "LastName"))
            .ThenBy(x => EF.Property<string>(x, "FirstName"))
            .ToListAsync();

        if (!entities.Any())
        {
            return new ActionResponse<IEnumerable<T>>
            {
                Message = "No existen registros con este criterio."
            };
        }

        return new ActionResponse<IEnumerable<T>>
        {
            WasSuccess = true,
            Result = entities
        };
    }

    public virtual async Task<ActionResponse<T>> UpdateAsync(T entity)
    {
        _context.Update(entity);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<T>
            {
                WasSuccess = true,
                Result = entity
            };
        }
        catch (DbUpdateException)
        {
            return DbUpdateExceptionActionResponse();
        }
        catch (Exception exception)
        {
            return ExceptionActionResponse(exception);
        }
    }

    private ActionResponse<T> ExceptionActionResponse(Exception exception)
    {
        return new ActionResponse<T>
        {
            Message = exception.Message
        };
    }

    private ActionResponse<T> DbUpdateExceptionActionResponse()
    {
        return new ActionResponse<T>
        {
            Message = "Ya existe el registro que está intentando crear."
        };
    }
}