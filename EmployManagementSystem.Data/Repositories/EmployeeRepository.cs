using EmployManagementSystem.Data.Context;
using EmployManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployManagementSystem.Data.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(EmployeManagementDbContext employeManagementDbContext) : base(employeManagementDbContext)
        {
        }

        public IEnumerable<Employee> Get()
        {
            return base.Get(
                orderBy: d => d.OrderBy(e => e.Id),
                includeProperties: "Department");
        }
        public override async Task<Employee> GetByID(long id)
        {
            IQueryable<Employee> query = dbSet;

            query = query.Where(e=>e.Id == id);

            query = query.Include("Department");

            return await query.FirstOrDefaultAsync();
        }

        public Employee FindByEmail(string emailAddress)
        {
            return base.Get(d => d.EmailAdress == emailAddress && d.IsActive,
                orderBy: d => d.OrderBy(e => e.Id)).FirstOrDefault();
        }

        //public override Employee GetByID(long id)
        //{
        //    return base.Get(d => d.Id == id, includeProperties: "Department")?.FirstOrDefault();
        //}


        public override void Insert(Employee entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow;
            entity.JoiningDate = DateTime.UtcNow;
            entity.IsActive = true;

            base.Insert(entity);
        }

        public override void Update(Employee entity)
        { 
            entity.ModifiedDate = DateTime.UtcNow;

            base.Update(entity);
        }

        public void DeleteEmployeesInDepartment(long departmentId)
        {
            var employees = base.Get(d => d.Department_id == departmentId ,
                orderBy: d => d.OrderBy(e => e.Id)).ToList();

            employees.ForEach(e=> base.Delete(e));
        }


    }

}
