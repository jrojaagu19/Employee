using Employees.Backend.UnitsOfWork.Interfaces;
using Employees.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Backend.Controllers;

public class GenericController<T> : Controller where T : class
{
    private readonly IGenericUnitOfWork<T> _unitOfWork;

    public GenericController(IGenericUnitOfWork<T> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginatedAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _unitOfWork.GetAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpGet("totalRecords")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _unitOfWork.GetTotalRecordsAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return NotFound(new { message = action.Message });
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new
            {
                message = "El campo de búsqueda es requerido."
            });
        }

        var action = await _unitOfWork.SearchAsync(query);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(new { message = action.Message });
    }

    [HttpPost]
    public virtual async Task<IActionResult> PostAsync(T model)
    {
        var action = await _unitOfWork.AddAsync(model);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpPut]
    public virtual async Task<IActionResult> PutAsync(T model)
    {
        var action = await _unitOfWork.UpdateAsync(model);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(int id)
    {
        var action = await _unitOfWork.DeleteAsync(id);
        if (action.WasSuccess)
        {
            return NoContent();
        }
        return BadRequest(action.Message);
    }
}