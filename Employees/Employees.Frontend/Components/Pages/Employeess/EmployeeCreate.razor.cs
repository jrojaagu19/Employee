using Employees.Frontend.Repositories;
using Employees.Shared.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Employees.Frontend.Components.Pages.Employeess;

public partial class EmployeeCreate
{
    private Employee employee = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private async Task CreateAsync()
    {
        var responseHttp = await Repository.PostAsync("api/employees", employee);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }

        Return();
        Snackbar.Add("Registro creado exitosamente.", Severity.Success);
    }

    private void Return()
    {
        NavigationManager.NavigateTo("/employees");
    }
}