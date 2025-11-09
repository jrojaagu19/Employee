using Employees.Backend.UnitsOfWork.Interfaces;
using Employees.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : GenericController<Category>
{
    public CategoriesController(IGenericUnitOfWork<Category> unit) : base(unit)
    {
    }
}