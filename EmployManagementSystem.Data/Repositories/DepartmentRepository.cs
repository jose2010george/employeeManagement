using EmployManagementSystem.Data.Context;
using EmployManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployManagementSystem.Data.Repositories
{

    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(EmployeManagementDbContext employeManagementDbContext) : base(employeManagementDbContext)
        {
        }

        public Department FindByName(string departmentName)
        {
            return base.Get(d => d.Name == departmentName && d.IsActive,
                orderBy: d => d.OrderBy(e => e.Id)).FirstOrDefault();
        }

        public IEnumerable<Department> Get()
        {
            return base.Get(        
                orderBy: d=>d.OrderBy(e=>e.Id), 
                includeProperties: "Employees");
        }

        public IEnumerable<Department> GetActiveDepartments()
        {
            return base.Get(d => d.IsActive,
                orderBy: d => d.OrderBy(e => e.Name));
        } 

        public override void Insert(Department entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow; 
            entity.IsActive = true;

            base.Insert(entity);
        }

        public override void Update(Department entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;

            base.Update(entity);
        }


    }

}
