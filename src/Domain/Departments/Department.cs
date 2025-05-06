using Domain.Employees;
using SharedKernel;

namespace Domain.Departments;

public sealed class Department : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
