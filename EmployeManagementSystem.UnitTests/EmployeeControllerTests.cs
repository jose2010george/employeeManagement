using EmployeManagementSystem.Controllers;
using EmployManagementSystem.Data.Repositories;
using Moq;
using NUnit.Framework;
using EmployManagementSystem.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace EmployeManagementSystem.UnitTests
{
    public class EmployeeControllerTests
    { 
        Mock<IEmployeeRepository> EmployeeRepositoryMock;
        Mock<IDepartmentRepository> DepartmentRepository; 
        EmployeeController _controller;

        [SetUp]
        public void Setup()
        { 
            EmployeeRepositoryMock = new Mock<IEmployeeRepository>();
            DepartmentRepository= new Mock<IDepartmentRepository>();
            _controller = new EmployeeController(EmployeeRepositoryMock.Object, DepartmentRepository.Object);
        }

        [Test]
        public void Employee_Create_Should_Retrun_Error_If_The_Email_Already_Exists()
        {
            //Arrange
            var employeeEmail = "joseTest@gmail.com";
            var objEmployee = GenerateEmployeeObj(employeeEmail, 100);
            EmployeeRepositoryMock.Setup(m => m.FindByEmail(employeeEmail)).Returns(objEmployee);

            //Act
            var result = (ViewResult)_controller.Create(objEmployee).Result;

            var modelStateEntry = result.ViewData.ModelState.Values.FirstOrDefault();

            var errorMessage = modelStateEntry?.Errors?.FirstOrDefault()?.ErrorMessage;


            //Assert
            Assert.AreEqual(errorMessage, "Email already already exists");
            Assert.True(!result.ViewData.ModelState.IsValid);
        }

        [Test]
        public void Department_Edit_Should_Retrun_Error_If_The_DepartmentName_Already_Exists()
        {
            //Arrange
            var employeeEmail = "joseTest@gmail.com";
            var objEmployee = GenerateEmployeeObj(employeeEmail, 100);
            var objEmployeeEdit = GenerateEmployeeObj(employeeEmail, 1012);
            EmployeeRepositoryMock.Setup(m => m.FindByEmail(employeeEmail)).Returns(objEmployee);

            //Act
            var result = (ViewResult)_controller.Edit(objEmployeeEdit.Id, objEmployeeEdit).Result;

            var modelStateEntry = result.ViewData.ModelState.Values.FirstOrDefault();

            var errorMessage = modelStateEntry?.Errors?.FirstOrDefault()?.ErrorMessage;


            //Assert
            Assert.AreEqual(errorMessage, "Email already already exists");
            Assert.True(!result.ViewData.ModelState.IsValid);
        }

        [Test]
        public void Department_Edit_Should_Call_Update_If_The_DepartmentName_Doesnot_Exists_For_Other_Departments()
        {
            //Arrange
            var employeeEmail = "joseTest@gmail.com";
            var objEmployee = GenerateEmployeeObj(employeeEmail, 100);
            var objEmployeeEdit = GenerateEmployeeObj(employeeEmail, 100);
            EmployeeRepositoryMock.Setup(m => m.FindByEmail(employeeEmail)).Returns(objEmployee);
            EmployeeRepositoryMock.Setup(m => m.Update(It.IsAny<Employee>()));
            EmployeeRepositoryMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            //Act 
            var actual = _controller.Edit(objEmployeeEdit.Id, objEmployeeEdit).Result;

            //Assert
            EmployeeRepositoryMock.Verify(mock => mock.Update(It.IsAny<Employee>()), Times.Once());
            EmployeeRepositoryMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }




        private Employee GenerateEmployeeObj(string email, long id)
        {
            var employee = new Employee { Id = id, EmailAdress = email, IsActive = true };
            employee.FirstName = "FistName";
            employee.LastName = "LastName";
            employee.PhoneNumber = "20987876756";
            employee.Address="Address";
            employee.City = "Test";
            employee.State = "Test";
            employee.ZipCode = "95356"; 
            employee.Password = "Test";
            employee.Department_id = 1;
            employee.JoiningDate = DateTime.UtcNow; 
            employee.CreatedDate = DateTime.UtcNow;
            employee.ModifiedDate = DateTime.UtcNow;
            return employee;
        }

    }

}