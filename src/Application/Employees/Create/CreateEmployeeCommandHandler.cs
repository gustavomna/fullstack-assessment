using Application.Abstractions.Messaging;
using Domain.Departments;
using Domain.Employees;
using SharedKernel;

namespace Application.Employees.Create;

internal sealed class CreateEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository) : ICommandHandler<CreateEmployeeCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var departments = await departmentRepository.GetAllAsync();
        var department = departments.FirstOrDefault(d => d.Name.Equals(command.Department, StringComparison.OrdinalIgnoreCase));

        if (department == null)
        {
            return Result.Failure<Guid>(EmployeeErrors.DepartmentNotFound(command.Department));
        }

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = command.FirstName,
            LastName = command.LastName,
            HireDate = command.HireDate,
            Phone = command.Phone,
            Address = command.Address,
            DepartmentId = department.Id
        };

        employee.Raise(new EmployeeCreatedDomainEvent(employee.Id));

        await employeeRepository.AddAsync(employee);

        return employee.Id;
    }
}
