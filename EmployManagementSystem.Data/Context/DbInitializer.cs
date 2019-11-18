using EmployManagementSystem.Data.Models;
using System;
using System.Linq;

namespace EmployManagementSystem.Data.Context
{
    public static class DbInitializer
    {
        public static void Initialize(EmployeManagementDbContext context)
        {
            //context.Database.EnsureCreated();

            // Look for any students.
            if (context.Departments.Any())
            {
                return;   // DB has been seeded
            }

            var departments = new Department[]
            {
                new Department { Name = "IT",  IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Department { Name = "HR",  IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            };

            foreach (Department d in departments)
            {
                context.Departments.Add(d);
            }
            context.SaveChanges();
        }
    }




}