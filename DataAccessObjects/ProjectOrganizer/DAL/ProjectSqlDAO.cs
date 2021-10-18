using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{


    public class ProjectSqlDAO : IProjectDAO
    {
        private const string SqlSelectAll = "SELECT * FROM dbo.project";

        private const string SqlInsertProjectEmployee = "INSERT INTO dbo.project_employee (project_id, employee_id) VALUES (@projectid, @employeeid)";

        private const string SqlInsertProject = "INSERT INTO dbo.project (name, from_date, to_date) VALUES (@name, @fromdate, @todate); SELECT @@IDENTITY";

        private const string SqlDeleteRow = "DELETE FROM dbo.project_employee WHERE project_id = @projectid AND employee_id = @employeeid";

        private readonly string connectionString;

        // Single Parameter Constructor
        public ProjectSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns all projects.
        /// </summary>
        /// <returns></returns>
        public ICollection<Project> GetAllProjects()
        {
            List<Project> results = new List<Project>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAll, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Project project = new Project
                        {
                         ProjectId = Convert.ToInt32(reader["project_id"]),
                         Name = Convert.ToString(reader["name"]),
                         StartDate = Convert.ToDateTime(reader["from_date"]),
                         EndDate = Convert.ToDateTime(reader["to_date"])
                        };

                        results.Add(project);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not query the database: " + ex.Message);
            }

            return results;
        }

        /// <summary>
        /// Assigns an employee to a project using their IDs.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlInsertProjectEmployee, conn);
                    command.Parameters.AddWithValue("@projectid", projectId);
                    command.Parameters.AddWithValue("@employeeid", employeeId);                   

                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not add employee to project: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Removes an employee from a project.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlDeleteRow, conn);
                    command.Parameters.AddWithValue("@projectid", projectId);
                    command.Parameters.AddWithValue("@employeeid", employeeId);

                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not remove employee from project: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="newProject">The new project object.</param>
        /// <returns>The new id of the project.</returns>
        public int CreateProject(Project newProject)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlInsertProject, conn);
                    command.Parameters.AddWithValue("@name", newProject.Name);
                    command.Parameters.AddWithValue("@fromdate", newProject.StartDate);
                    command.Parameters.AddWithValue("@todate", newProject.EndDate);

                    int id = Convert.ToInt32(command.ExecuteScalar());

                    return id;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not create department: " + ex.Message);
                return -1;
            }
        }

    }
}
