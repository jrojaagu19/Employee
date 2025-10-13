using Employees.Frontend.Repositories;
using Employees.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace Employees.Frontend.Components.Pages.Employees
{
    public partial class EmployeesIndex
    {
        [Inject] private IRepository Repository { get; set; } = null!;
        private List<Employee>? employees;

        protected override async Task OnInitializedAsync()
        {
            var httpResult = await Repository.GetAsync<List<Employee>>("api/employees");
            employees = httpResult.Response;
        }
    }
}