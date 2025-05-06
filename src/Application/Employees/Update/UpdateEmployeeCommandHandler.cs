using Application.Abstractions.Messaging;
using Domain.Departments;
using Domain.Employees;
using SharedKernel;

namespace Application.Employees.Update;

internal sealed class UpdateEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository) : ICommandHandler<UpdateEmployeeCommand>
{
    public async Task<Result> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(command.Id);

        if (employee is null)
        {
            return Result.Failure(EmployeeErrors.NotFound(command.Id));
        }

        var departments = await departmentRepository.GetAllAsync();
        var department = departments.FirstOrDefault(d => d.Name.Equals(command.Department, StringComparison.OrdinalIgnoreCase));

        if (department is null)
        {
            return Result.Failure(EmployeeErrors.DepartmentNotFound(command.Department));
        }

        employee.FirstName = command.FirstName;
        employee.LastName = command.LastName;
        employee.Phone = command.Phone;
        employee.Address = command.Address;
        employee.DepartmentId = department.Id;

        await employeeRepository.UpdateAsync(employee);

        return Result.Success();
    }
}
