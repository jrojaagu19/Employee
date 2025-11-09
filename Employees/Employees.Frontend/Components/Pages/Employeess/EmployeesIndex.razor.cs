using Employees.Frontend.Components.Shared;
using Employees.Frontend.Repositories;
using Employees.Shared.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Net;

namespace Employees.Frontend.Components.Pages.Employeess;

public partial class EmployeesIndex
{
    private MudTable<Employee>? table;
    private readonly int[] pageSizeOptions = { 10, 25, 50, 100 };
    private int totalRecords = 0;
    private bool loading = false;
    private const string baseUrl = "api/employees";
    private string infoFormat = "{first_item}-{last_item} de {all_items}";

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && table != null)
        {
            await table.ReloadServerData();
        }
    }

    private async Task<TableData<Employee>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        Console.WriteLine("🚀 LoadListAsync iniciado");
        loading = true;
        StateHasChanged();

        try
        {
            int page = state.Page + 1;
            int pageSize = state.PageSize;

            Console.WriteLine($"📄 Page: {page}, PageSize: {pageSize}");

            var totalUrl = $"{baseUrl}/totalRecords?page={page}&recordsNumber={pageSize}";
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                totalUrl += $"&filter={Uri.EscapeDataString(Filter)}";
            }

            Console.WriteLine($"🔍 URL totalRecords: {totalUrl}");

            var totalResponse = await Repository.GetAsync<int>(totalUrl);
            if (!totalResponse.Error)
            {
                totalRecords = totalResponse.Response;
                Console.WriteLine($"✅ Total records: {totalRecords}");
            }
            else
            {
                var errorMsg = await totalResponse.GetErrorMessageAsync();
                Console.WriteLine($"❌ Error totalRecords: {errorMsg}");
                Snackbar.Add($"Error al obtener total: {errorMsg}", Severity.Error);
            }

            var url = $"{baseUrl}/paginated?page={page}&recordsNumber={pageSize}";
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                url += $"&filter={Uri.EscapeDataString(Filter)}";
            }

            Console.WriteLine($"🔍 URL paginated: {url}");

            var responseHttp = await Repository.GetAsync<List<Employee>>(url);

            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Console.WriteLine($"❌ Error paginated: {message}");
                Console.WriteLine($"❌ Status Code: {responseHttp.HttpResponseMessage?.StatusCode}");
                Snackbar.Add($"Error al cargar empleados: {message}", Severity.Error);
                return new TableData<Employee> { Items = new List<Employee>(), TotalItems = 0 };
            }

            if (responseHttp.Response == null)
            {
                Console.WriteLine("⚠️ Response es null");
                return new TableData<Employee> { Items = new List<Employee>(), TotalItems = 0 };
            }

            Console.WriteLine($"✅ Empleados obtenidos: {responseHttp.Response.Count}");

            foreach (var emp in responseHttp.Response.Take(3))
            {
                Console.WriteLine($"👤 Empleado: {emp.Id} - {emp.FirstName} {emp.LastName}");
            }

            return new TableData<Employee>
            {
                Items = responseHttp.Response,
                TotalItems = totalRecords
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Exception: {ex.Message}");
            Console.WriteLine($"💥 StackTrace: {ex.StackTrace}");
            Snackbar.Add($"Error inesperado: {ex.Message}", Severity.Error);
            return new TableData<Employee> { Items = new List<Employee>(), TotalItems = 0 };
        }
        finally
        {
            loading = false;
            StateHasChanged();
            Console.WriteLine("🏁 LoadListAsync finalizado");
        }
    }

    private async Task SetFilterValue(string value)
    {
        Filter = value;
        if (table != null)
        {
            await table.ReloadServerData();
        }
    }

    private async Task ShowModalAsync(int id = 0, bool isEdit = false)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true
        };

        IDialogReference? dialog;

        if (isEdit)
        {
            var parameters = new DialogParameters
            {
                { "Id", id }
            };
            dialog = await DialogService.ShowAsync<EmployeeEdit>("Editar Empleado", parameters, options);
        }
        else
        {
            dialog = await DialogService.ShowAsync<EmployeeCreate>("Nuevo Empleado", options);
        }

        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            if (table != null)
            {
                await table.ReloadServerData();
            }
        }
    }

    private async Task DeleteAsync(Employee employee)
    {
        var parameters = new DialogParameters
        {
            { "Message", $"¿Estás seguro de borrar el empleado: {employee.FirstName} {employee.LastName}?" }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.ExtraSmall,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirmación", parameters, options);
        var result = await dialog.Result;

        if (result!.Canceled)
            return;

        var responseHttp = await Repository.DeleteAsync($"{baseUrl}/{employee.Id}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage?.StatusCode == HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/employees");
            }
            else
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(message!, Severity.Error);
            }
            return;
        }

        if (table != null)
        {
            await table.ReloadServerData();
        }
        Snackbar.Add("Empleado eliminado correctamente", Severity.Success);
    }
}