
namespace Domain.Departments;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(Guid id);
    Task<Department> AddAsync(Department department);
    Task UpdateAsync(Department department);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
