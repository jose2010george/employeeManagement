using EmployManagementSystem.Data.Models;
using System.Collections.Generic;

namespace EmployManagementSystem.Data.Repositories
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        IEnumerable<Department> Get();

        IEnumerable<Department> GetActiveDepartments();
        Department FindByName(string departmentName);
    }

}
