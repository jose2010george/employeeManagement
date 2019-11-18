using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployManagementSystem.Data.Models;
using EmployManagementSystem.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ILogger<DepartmentController> _logger;
        private IDepartmentRepository _repository;
        private IEmployeeRepository _employeeRepository;
        public DepartmentController(IDepartmentRepository repository, IEmployeeRepository employeeRepository)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
        }
         
        public ActionResult Index()
        {
            return View(_repository.Get());
        }



        public IActionResult Create()
        {
            return View(new Department());
        }

     

        [HttpPost, ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create([Bind("Name")] Department department)
        {
            if (ModelState.IsValid)
            {                
                var departmentWithName = _repository.FindByName(department.Name);
                if (departmentWithName == null)
                {
                    _repository.Insert(department);
                    await _repository.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "Department name already already exists");
                }
            }           
            return View(department);
        }

         
        public async Task<IActionResult> Edit(long id)
        {
            var department = await _repository.GetByID(id);
            return View(department);
        }
         
        [HttpPost, ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,CreatedDate,IsActive")] Department department)
        {  
            try
            {
                if (ModelState.IsValid)
                {
                    var departmentWithName = _repository.FindByName(department.Name);
                    if (departmentWithName == null || (departmentWithName != null && departmentWithName.Id == id))
                    {
                        _repository.Update(department);
                        await _repository.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("Name", "Department name already already exists");
                    }
                }
            }
            catch(Exception ex)
            {
                var exception = ex;
            }
            return View();
        }
         
        public async Task<IActionResult> Delete(long id)
        {
            var department = await _repository.GetByID(id);
            return View(department);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id, [Bind("Id")] Department department)
        {
            try
            {
                _employeeRepository.DeleteEmployeesInDepartment(department.Id);
                _repository.Delete(department.Id);
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