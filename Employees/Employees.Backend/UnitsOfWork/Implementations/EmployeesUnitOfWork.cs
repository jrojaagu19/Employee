using Employees.Backend.Repositories.Interfaces;
using Employees.Backend.UnitsOfWork.Interfaces;
using Employees.Shared.Entities;
using Employees.Shared.Responses;

namespace Employees.Backend.UnitsOfWork.Implementations;

public class EmployeesUnitOfWork : IEmployeesUnitOfWork
{
    private readonly IEmployeesRepository _repository;

    public EmployeesUnitOfWork(IEmployeesRepository repository)
    {
        _repository = repository;
    }

    public Task<ActionResponse<Employee>> AddAsync(Employee entity) => _repository.AddAsync(entity);

    public Task<ActionResponse<Employee>> DeleteAsync(int id) => _repository.DeleteAsync(id);

    public Task<ActionResponse<IEnumerable<Employee>>> GetAsync() => _repository.GetAsync();

    public Task<ActionResponse<Employee>> GetAsync(int id) => _repository.GetAsync(id);

    public Task<ActionResponse<Employee>> UpdateAsync(Employee entity) => _repository.UpdateAsync(entity);

    public Task<ActionResponse<IEnumerable<Employee>>> GetByNameAsync(string name) => _repository.GetByNameAsync(name);
}