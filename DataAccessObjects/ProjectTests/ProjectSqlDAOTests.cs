using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTests
{
    [TestClass]
    public class ProjectSqlDAOTests : ProjectTestsBase
    {
        [TestMethod]
        public void GetAllProjects_Should_ReturnAllEmployees()
        {
            // Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(this.ConnectionString);

            // Act
            ICollection<Project> result = dao.GetAllProjects();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void AssignEmployeeToProject_ShouldReturnFalseIfProjectDoesNotExist()
        {
            // Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);

            // Act
            bool result = dao.AssignEmployeeToProject(1, 2);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, GetRowCount("dbo.project_employee"));
        }

        [TestMethod]
        public void RemoveEmployeeFromProject_ShouldShowZeroRows()
        {
            // Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);

            // Act
            bool result = dao.RemoveEmployeeFromProject(1, 1);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, GetRowCount("dbo.project_employee"));
        }

        [TestMethod]

        public void CreateProject__Should_IncreaseCountBy1()
        {
            // Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);

            Project project = new Project
            {
                Name = "New",
                ProjectId = 1,
                StartDate = Convert.ToDateTime("01/20/2021"),
                EndDate = Convert.ToDateTime("12/15/2021")
            };

            // Act
            int result = dao.CreateProject(project);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, GetRowCount("project"));
        }
    }
}
