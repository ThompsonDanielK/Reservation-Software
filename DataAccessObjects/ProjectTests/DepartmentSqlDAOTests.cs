using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTests
{
    [TestClass]
    public class DepartmentSqlDAOTests : ProjectTestsBase 
    {
        [TestMethod]
        public void GetDepartmentsTest_Should_ReturnAllDepartmetns()
        {
            // Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(this.ConnectionString);

            // Act
            ICollection<Department> result = dao.GetDepartments();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void CreateDepartment_Should_IncreaseCountBy1()
        {
            // Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);
            Department department = new Department();
            department.Id = 2;
            department.Name = "DotNet";

            // Act
            int id = dao.CreateDepartment(department);

            // Assert
            Assert.IsTrue(id > 1, "Added department ID looks to be invalid");
            Assert.AreEqual(2, GetRowCount("dbo.department"));
        }

        [TestMethod]
        public void UpdateDepartment_Should_ReturnTrue()
        {
            // Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);
            Department department = new Department();
            department.Id = 3;
            department.Name = "CodeReview";

            // Act
            bool result = dao.UpdateDepartment(department);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
