using ProjectUnitOfWork.Core.Interfaces;
using ProjectUnitOfWork.Core.Models;

namespace ProjectUnitOfWork.Core;
public interface IUnitOfWork : IDisposable
{
    IBaseRepository<Employee> Employees { get; }

    int Complete();
}
