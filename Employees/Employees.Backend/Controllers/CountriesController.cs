using Employees.Backend.Data;
using Employees.Backend.UnitsOfWork.Interfaces;
using Employees.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employees.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : GenericController<Country>
{
    public CountriesController(IGenericUnitOfWork<Country> unit) : base(unit)
    {
    }
}