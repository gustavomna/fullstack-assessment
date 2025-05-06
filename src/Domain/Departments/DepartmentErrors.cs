using SharedKernel;

namespace Domain.Departments;

public static class DepartmentErrors
{
    public static Error NotFound(Guid departmentId) => Error.NotFound(
        "Departments.NotFound",
        $"O departamento com o Id = '{departmentId}' não foi encontrado");

    public static Error HasEmployees(Guid departmentId) => Error.Conflict(
        "Departments.HasEmployees",
        $"O departamento com Id = '{departmentId}' possui funcionários e não pode ser excluído");

    public static readonly Error NameExists = Error.Conflict(
        "Departments.NameExists",
        "Já existe um departamento com este nome");

    public static Error Unauthorized() => Error.Failure(
        "Departments.Unauthorized",
        "Você não está autorizado a realizar esta ação.");
}
