using SharedKernel;

namespace Domain.Employees;

public static class EmployeeErrors
{
    public static Error NotFound(Guid employeeId) => Error.NotFound(
        "Employees.NotFound",
        $"O funcionário com o Id = '{employeeId}' não foi encontrado");

    public static Error Unauthorized() => Error.Failure(
        "Employees.Unauthorized",
        "Você não está autorizado a realizar esta ação.");

    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Employees.NotFoundByEmail",
        "O funcionário com o e-mail especificado não foi encontrado");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Employees.EmailNotUnique",
        "O e-mail fornecido já está em uso por outro funcionário");

    public static readonly Error InactiveEmployee = Error.Problem(
        "Employees.Inactive",
        "Este funcionário está inativo e não pode realizar esta operação");

    public static Error DepartmentNotFound(string departmentName) => Error.NotFound(
        "Employees.DepartmentNotFound",
        $"O departamento com nome '{departmentName}' não foi encontrado");
}
