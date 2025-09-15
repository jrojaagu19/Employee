using Employees.Backend.UnitsOfWork.Interfaces;
using Employees.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeesUnitOfWork _unitOfWork;

    public EmployeesController(IEmployeesUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var response = await _unitOfWork.GetAsync();
        return Ok(response.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var response = await _unitOfWork.GetAsync(id);
        if (!response.WasSuccess) return NotFound(response.Message);
        return Ok(response.Result);
    }

    [HttpGet("search/{text}")]
    public async Task<IActionResult> GetByNameAsync(string text)
    {
        var response = await _unitOfWork.GetByNameAsync(text);
        return Ok(response.Result);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Employee employee)
    {
        var response = await _unitOfWork.AddAsync(employee);
        if (!response.WasSuccess) return BadRequest(response.Message);
        return Ok(response.Result);
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync(Employee employee)
    {
        var response = await _unitOfWork.UpdateAsync(employee);
        if (!response.WasSuccess) return BadRequest(response.Message);
        return Ok(response.Result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _unitOfWork.DeleteAsync(id);
        if (!response.WasSuccess) return NotFound(response.Message);
        return NoContent();
    }
}