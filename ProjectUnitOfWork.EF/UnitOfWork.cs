using ProjectUnitOfWork.Core;
using ProjectUnitOfWork.Core.Interfaces;
using ProjectUnitOfWork.Core.Models;
using ProjectUnitOfWork.EF.Repositories;

namespace ProjectUnitOfWork.EF;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IBaseRepository<Employee> Employees { get; private set; }
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Employees = new BaseRepository<Employee>(_context);

    }

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}