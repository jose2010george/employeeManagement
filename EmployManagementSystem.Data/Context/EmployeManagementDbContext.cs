using EmployManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployManagementSystem.Data.Context
{ 
    public class EmployeManagementDbContext : DbContext
    { 
        public EmployeManagementDbContext(DbContextOptions<EmployeManagementDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; } 
        public virtual DbSet<Employee> Employees { get; set; }
        
        public override int SaveChanges()
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);
            }

            return base.SaveChanges();
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);
            }

            return (await base.SaveChangesAsync(true, cancellationToken));
        }
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            
            modelBuilder.Entity<Department>()
                .Property(e => e.Name)
                .IsUnicode(false);


            modelBuilder.Entity<Employee>()
                .Property(e => e.Department_id)
                .IsRequired();

            modelBuilder.Entity<Department>()
               .HasMany(e => e.Employees)
               .WithOne(d=>d.Department)
               .HasForeignKey(e => e.Department_id)
               .IsRequired();             

            modelBuilder.Entity<Employee>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmailAdress)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.ZipCode)
                .IsUnicode(false);
             
            modelBuilder.Entity<Employee>()
                .Property(e => e.Password)
                .IsUnicode(false);
        }
    }
     
}