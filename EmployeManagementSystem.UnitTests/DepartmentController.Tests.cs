using EmployeManagementSystem.Controllers;
using EmployManagementSystem.Data.Repositories;
using Moq;
using NUnit.Framework;
using EmployManagementSystem.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace EmployeManagementSystem.UnitTests
{
    public class DepartmentControllerTests
    {
        Mock<IDepartmentRepository> DepartmentRepositoryMock;
        Mock<IEmployeeRepository> EmployeeRepositoryMock;
        DepartmentController _controller;

        [SetUp]
        public void Setup()
        {
            DepartmentRepositoryMock = new Mock<IDepartmentRepository>(); 
            EmployeeRepositoryMock = new Mock<IEmployeeRepository>();
            _controller = new DepartmentController(DepartmentRepositoryMock.Object, EmployeeRepositoryMock.Object);
        }

        [Test]
        public void Department_Create_Should_Retrun_Error_If_The_DepartmentName_Already_Exists()
        {
            //Arrange
            var departmentName = "TestDepartment";
            var objDepartment = new Department() { Name = departmentName, Id = 100, IsActive = true };
            DepartmentRepositoryMock.Setup(m => m.FindByName(departmentName)).Returns(objDepartment);
            
            //Act
            var result = (ViewResult)_controller.Create(objDepartment).Result;

            var modelStateEntry = result.ViewData.ModelState.Values.FirstOrDefault();
         
            var errorMessage = modelStateEntry?.Errors?.FirstOrDefault()?.ErrorMessage;
            

            //Assert
            Assert.AreEqual(errorMessage, "Department name already already exists");
            Assert.True(!result.ViewData.ModelState.IsValid);
        }

        [Test]
        public void Department_Edit_Should_Retrun_Error_If_The_DepartmentName_Already_Exists()
        {
            //Arrange
            var departmentName = "TestDepartment";
            var objDepartment = new Department() { Name = departmentName, Id = 100, IsActive = true };
            var objDepartmentForEdit = new Department() { Name = departmentName, Id = 110, IsActive = true };
            DepartmentRepositoryMock.Setup(m => m.FindByName(departmentName)).Returns(objDepartment);

            //Act
            var result = (ViewResult)_controller.Edit(objDepartmentForEdit.Id, objDepartmentForEdit).Result;

            var modelStateEntry = result.ViewData.ModelState.Values.FirstOrDefault();

            var errorMessage = modelStateEntry?.Errors?.FirstOrDefault()?.ErrorMessage;


            //Assert
            Assert.AreEqual(errorMessage, "Department name already already exists");
            Assert.True(!result.ViewData.ModelState.IsValid);
        }

        [Test]
        public void Department_Edit_Should_Call_Update_If_The_DepartmentName_Doesnot_Exists_For_Other_Departments()
        {
            //Arrange
            var departmentName = "TestDepartment";
            var objDepartment = new Department() { Name = departmentName, Id = 100, IsActive = true };
            var objDepartmentForEdit = new Department() { Name = departmentName, Id = 100, IsActive = true };
            DepartmentRepositoryMock.Setup(m => m.FindByName(departmentName)).Returns(objDepartment);
            DepartmentRepositoryMock.Setup(m => m.Update(It.IsAny<Department>())); 
            DepartmentRepositoryMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1)); 

            //Act 
            var actual = _controller.Edit(objDepartmentForEdit.Id, objDepartmentForEdit).Result;

            //Assert
            DepartmentRepositoryMock.Verify(mock => mock.Update(It.IsAny<Department>()), Times.Once());
            DepartmentRepositoryMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }

}