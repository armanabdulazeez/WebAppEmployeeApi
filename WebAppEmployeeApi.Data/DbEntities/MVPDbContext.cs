using Microsoft.EntityFrameworkCore;
using WebAppEmployeeApi.Data.Entities;

namespace WebAppEmployeeApi.Data.DbEntities
{
    public class MVPDbContext : DbContext
    {
        public MVPDbContext(DbContextOptions<MVPDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeAddress> EmployeesAddress { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<Employee>(e => e.UserId);

        }
    }
}
