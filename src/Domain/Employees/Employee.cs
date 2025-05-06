using Domain.Departments;
using SharedKernel;

namespace Domain.Employees;

public sealed class Employee : Entity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime HireDate { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }

    public Guid DepartmentId { get; set; }
    public Department Department { get; set; }
}

