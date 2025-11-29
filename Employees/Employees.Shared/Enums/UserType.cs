using System.ComponentModel;

namespace Employees.Shared.Enums;

public enum UserType
{
    [Description("Administrador")]
    Admin,

    [Description("Usuario")]
    User
}