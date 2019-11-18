using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployManagementSystem.Data.Models;
using EmployManagementSystem.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace EmployeManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private const string DepartmentID = "DepartmentID";
        private readonly ILogger<EmployeeController> _logger;
        private IEmployeeRepository _repository;
        private IDepartmentRepository _departmentRepository;
        public EmployeeController(IEmployeeRepository repository, IDepartmentRepository departmentRepository)
        {
            _repository = repository;
            _departmentRepository = departmentRepository;
        }

        public ActionResult Index()
        {
            return View(_repository.Get());
        }
 
        public ActionResult Create()
        {
            ViewData[DepartmentID] = new SelectList(_departmentRepository.GetActiveDepartments(), "Id", "Name");
            return View(new Employee());
        }

        [HttpPost, ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create([Bind("FirstName ,LastName ,EmailAdress ,PhoneNumber ,Address ,City ,State ,ZipCode ,UserName ,Password ,Department_id")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var employeeWithEmail = _repository.FindByEmail(employee.EmailAdress);
                if(employeeWithEmail==null)
                {
                    _repository.Insert(employee);
                    await _repository.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("EmailAdress", "Email already already exists");
                }              
            }
            ViewData[DepartmentID] = new SelectList(_departmentRepository.GetActiveDepartments(), "Id", "Name");
            return View(employee);
        }
        
        public async Task<IActionResult> Edit(long id)
        {
            ViewData[DepartmentID] = new SelectList(_departmentRepository.GetActiveDepartments(), "Id", "Name");
            var employee = await _repository.GetByID(id);
            return View(employee);
        }
        
        public async Task<IActionResult> Details(long id)
        {
            var employee = await _repository.GetByID(id);
            return View(employee);
        }

        [HttpPost, ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit(long id, [Bind("Id,FirstName ,LastName ,EmailAdress ,PhoneNumber ,Address ,City ,State ,ZipCode ,UserName ,Password ,Department_id, JoiningDate ,IsActive ,CreatedDate")] Employee employee)
        {
            try
            {  
                if (ModelState.IsValid)
                {
                    var employeeWithEmail = _repository.FindByEmail(employee.EmailAdress);
                    if (employeeWithEmail == null || (employeeWithEmail!=null && employeeWithEmail.Id == id))
                    {
                        _repository.Update(employee);
                        await _repository.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("EmailAdress", "Email already already exists");
                    }
                }
            }
            catch (Exception ex)
            {
                var exception = ex;
            }
            ViewData[DepartmentID] = new SelectList(_departmentRepository.GetActiveDepartments(), "Id", "Name");
            return View();
        }
         
        public async Task<IActionResult> Delete(long id)
        {
            var employee = await _repository.GetByID(id);
            return View(employee);
        }
         
        [HttpPost, ValidateAntiForgeryToken] 
        public async Task<IActionResult> Delete(long id, [Bind("Id")] Employee employee)
        {
            try
            {
                _repository.Delete(employee.Id);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var exception = ex;
            }
            return View();
        }
    }
}