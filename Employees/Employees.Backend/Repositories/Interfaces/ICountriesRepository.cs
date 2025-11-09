using Employees.Shared.Entities;
using Employees.Shared.Responses;

namespace Employees.Backend.Repositories.Interfaces;

public interface ICountriesRepository
{
    Task<ActionResponse<Country>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Country>>> GetAsync();
}