using Domain.Employees;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await DbContext.Set<Employee>()
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await DbContext.Set<Employee>()
            .Include(e => e.Department)
            .ToListAsync();
    }

    public async Task AddAsync(Employee employee)
    {
        await DbContext.Set<Employee>().AddAsync(employee);
        await DbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        DbContext.Set<Employee>().Update(employee);
        await DbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var employee = await GetByIdAsync(id);
        if (employee != null)
        {
            DbContext.Set<Employee>().Remove(employee);
            await DbContext.SaveChangesAsync();
        }
    }
}
