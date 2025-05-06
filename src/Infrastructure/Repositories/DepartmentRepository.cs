using Domain.Departments;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class DepartmentRepository : Repository<Department>, IDepartmentRepository
{
    public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Department> AddAsync(Department department)
    {
        await DbContext.Departments.AddAsync(department);
        await DbContext.SaveChangesAsync();
        return department;
    }

    public async Task DeleteAsync(Guid id)
    {
        var department = await DbContext.Departments.FindAsync(id);
        if (department != null)
        {
            DbContext.Departments.Remove(department);
            await DbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await DbContext.Departments.AnyAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await DbContext.Departments
            .Include(d => d.Employees)
            .ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(Guid id)
    {
        return await DbContext.Departments
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task UpdateAsync(Department department)
    {
        DbContext.Departments.Update(department);
        await DbContext.SaveChangesAsync();
    }
}
