using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employees.Shared.Entities;

public class Employee
{
    public int Id { get; set; }

    [Display(Name = "Nombre")]
    [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Apellido")]
    [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Activo")]
    public bool IsActive { get; set; }

    [Display(Name = "Fecha de contratación")]
    public DateTime HireDate { get; set; }

    [Display(Name = "Salario")]
    [Range(1000000, double.MaxValue, ErrorMessage = "El salario debe ser mínimo $1,000,000.")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }
}