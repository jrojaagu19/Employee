using Employees.Backend.Repositories.Interfaces;
using Employees.Shared.Entities;
using Employees.Shared.Responses;

namespace Employees.Backend.UnitsOfWork.Interfaces;

public interface IEmployeesUnitOfWork : IGenericRepository<Employee>
{
    Task<ActionResponse<IEnumerable<Employee>>> GetByNameAsync(string name);
}