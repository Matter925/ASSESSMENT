using Microsoft.EntityFrameworkCore;

using ProjectUnitOfWork.Core.Models;


namespace ProjectUnitOfWork.EF;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<Employee> Employees { get; set; }
}