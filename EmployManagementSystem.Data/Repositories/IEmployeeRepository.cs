using EmployManagementSystem.Data.Models;
using System.Collections.Generic;

namespace EmployManagementSystem.Data.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        IEnumerable<Employee> Get();
        Employee FindByEmail(string emailAddress);
        void DeleteEmployeesInDepartment(long departmentId);
    }

}
