using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ProjectTests
{
    [TestClass]
    public class EmployeeSqlDAOTests : ProjectTestsBase
    {
        [TestMethod]
        public void GetAllEmployeesTest_Should_ReturnAllEmployees()
        {
            // Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(this.ConnectionString);

            // Act
            ICollection<Employee> result = dao.GetAllEmployees();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Search_Should_ReturnZeroRowsIfEmployeeDoesNotExist()
        {
            // Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(ConnectionString);
            
            // Act
            ICollection<Employee> result = dao.Search("Leeroy", "Earnhardt");

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetEmployeesWithoutProjects_Should_ReturnZero()
        {
            // Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(ConnectionString);

            // Act
            ICollection<Employee> result = dao.GetEmployeesWithoutProjects();

            // Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}
